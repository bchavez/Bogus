namespace Bogus.Compression {
#region license

/*
Copyright (c) 2013, Milosz Krajewski
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided 
that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions 
  and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
  and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR 
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE 
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#endregion

using System;
using System.IO;

namespace LZ4
{
	// ReSharper disable once PartialTypeWithSinglePart
	/// <summary>Block compression stream. Allows to use LZ4 for stream compression.</summary>
	public partial class LZ4Stream: Stream
	{
		#region ChunkFlags

		/// <summary>
		/// Flags of a chunk. Please note, this 
		/// </summary>
		[Flags]
		public enum ChunkFlags
		{
			/// <summary>None.</summary>
			None = 0x00,

			/// <summary>Set if chunk is compressed.</summary>
			Compressed = 0x01,

			/// <summary>Set if high compression has been selected (does not affect decoder, 
			/// but might be useful when rewriting)</summary>
			HighCompression = 0x02,

			/// <summary>3 bits for number of passes. Currently only 1 pass (value 0) 
			/// is supported.</summary>
			Passes = 0x04 | 0x08 | 0x10, // not used currently
		}

		#endregion

		#region fields

		/// <summary>The inner stream.</summary>
		private readonly Stream _innerStream;

		/// <summary>The compression mode.</summary>
		private readonly LZ4StreamMode _compressionMode;

		/// <summary>The high compression flag (compression only).</summary>
		private readonly bool _highCompression;

		/// <summary>Determines if reading tries to return something ASAP or wait 
		/// for full chunk (decompression only).</summary>
		private readonly bool _interactiveRead;

		/// <summary>Isolates inner stream which will not be closed 
		/// when this stream is closed.</summary>
		private readonly bool _isolateInnerStream;

		/// <summary>The block size (compression only).</summary>
		private readonly int _blockSize;

		/// <summary>The buffer.</summary>
		private byte[] _buffer;

		/// <summary>The buffer length (can be different then _buffer.Length).</summary>
		private int _bufferLength;

		/// <summary>The offset in a buffer.</summary>
		private int _bufferOffset;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="LZ4Stream" /> class.</summary>
		/// <param name="innerStream">The inner stream.</param>
		/// <param name="compressionMode">The compression mode.</param>
		/// <param name="highCompression">if set to <c>true</c> high compression is used.</param>
		/// <param name="blockSize">Size of the block.</param>
		/// <param name="interactiveRead">if set to <c>true</c> interactive read mode is used. 
		/// It means that <see cref="Read"/> method tries to return data as soon as possible. 
		/// Please note, that this should be default behavior but has been made optional for 
		/// backward compatibility. This constructor will be changed in next major release.</param>
		[Obsolete("This constructor is obsolete")]
		public LZ4Stream(
			Stream innerStream,
			LZ4StreamMode compressionMode,
			bool highCompression,
			int blockSize = 1024*1024,
			bool interactiveRead = false)
		{
			_innerStream = innerStream;
			_compressionMode = compressionMode;
			_highCompression = highCompression;
			_interactiveRead = interactiveRead;
			_isolateInnerStream = false;
			_blockSize = Math.Max(16, blockSize);
		}

		/// <summary>Initializes a new instance of the <see cref="LZ4Stream" /> class.</summary>
		/// <param name="innerStream">The inner stream.</param>
		/// <param name="compressionMode">The compression mode.</param>
		/// <param name="compressionFlags">The compression flags.</param>
		/// <param name="blockSize">Size of the block.</param>
		public LZ4Stream(
			Stream innerStream,
			LZ4StreamMode compressionMode,
			LZ4StreamFlags compressionFlags = LZ4StreamFlags.Default,
			int blockSize = 1024*1024)
		{
			_innerStream = innerStream;
			_compressionMode = compressionMode;
			_highCompression = (compressionFlags & LZ4StreamFlags.HighCompression) != 0;
			_interactiveRead = (compressionFlags & LZ4StreamFlags.InteractiveRead) != 0;
			_isolateInnerStream = (compressionFlags & LZ4StreamFlags.IsolateInnerStream) != 0;
			_blockSize = Math.Max(16, blockSize);
		}

		#endregion

		#region utilities

		/// <summary>Returns NotSupportedException.</summary>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns>NotSupportedException</returns>
		private static NotSupportedException NotSupported(string operationName)
		{
			return new NotSupportedException(
				string.Format(
					"Operation '{0}' is not supported", operationName));
		}

		/// <summary>Returns EndOfStreamException.</summary>
		/// <returns>EndOfStreamException</returns>
		private static EndOfStreamException EndOfStream()
		{
			return new EndOfStreamException("Unexpected end of stream");
		}

		/// <summary>Tries to read variable length int.</summary>
		/// <param name="result">The result.</param>
		/// <returns><c>true</c> if integer has been read, <c>false</c> if end of stream has been
		/// encountered. If end of stream has been encountered in the middle of value 
		/// <see cref="EndOfStreamException"/> is thrown.</returns>
		private bool TryReadVarInt(out ulong result)
		{
			var buffer = new byte[1];
			var count = 0;
			result = 0;

			while (true)
			{
				if (_innerStream.Read(buffer, 0, 1) == 0)
				{
					if (count == 0) return false;
					throw EndOfStream();
				}
				var b = buffer[0];
				result = result + ((ulong)(b & 0x7F) << count);
				count += 7;
				if ((b & 0x80) == 0 || count >= 64) break;
			}

			return true;
		}

		/// <summary>Reads the variable length int. Work with assumption that value is in the stream
		/// and throws exception if it isn't. If you want to check if value is in the stream
		/// use <see cref="TryReadVarInt"/> instead.</summary>
		/// <returns></returns>
		private ulong ReadVarInt()
		{
			ulong result;
			if (!TryReadVarInt(out result)) throw EndOfStream();
			return result;
		}

		/// <summary>Reads the block of bytes. 
		/// Contrary to <see cref="Stream.Read"/> does not read partial data if possible. 
		/// If there is no data (yet) it waits.</summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <returns>Number of bytes read.</returns>
		private int ReadBlock(byte[] buffer, int offset, int length)
		{
			var total = 0;

			while (length > 0)
			{
				var read = _innerStream.Read(buffer, offset, length);
				if (read == 0) break;
				offset += read;
				length -= read;
				total += read;
			}

			return total;
		}

		/// <summary>Writes the variable length integer.</summary>
		/// <param name="value">The value.</param>
		private void WriteVarInt(ulong value)
		{
			var buffer = new byte[1];
			while (true)
			{
				var b = (byte)(value & 0x7F);
				value >>= 7;
				buffer[0] = (byte)(b | (value == 0 ? 0 : 0x80));
				_innerStream.Write(buffer, 0, 1);
				if (value == 0) break;
			}
		}

		/// <summary>Flushes current chunk.</summary>
		private void FlushCurrentChunk()
		{
			if (_bufferOffset <= 0) return;

			var compressed = new byte[_bufferOffset];
			var compressedLength = _highCompression
				? LZ4Codec.EncodeHC(_buffer, 0, _bufferOffset, compressed, 0, _bufferOffset)
				: LZ4Codec.Encode(_buffer, 0, _bufferOffset, compressed, 0, _bufferOffset);

			if (compressedLength <= 0 || compressedLength >= _bufferOffset)
			{
				// incompressible block
				compressed = _buffer;
				compressedLength = _bufferOffset;
			}

			var isCompressed = compressedLength < _bufferOffset;

			var flags = ChunkFlags.None;

			if (isCompressed) flags |= ChunkFlags.Compressed;
			if (_highCompression) flags |= ChunkFlags.HighCompression;

			WriteVarInt((ulong)flags);
			WriteVarInt((ulong)_bufferOffset);
			if (isCompressed) WriteVarInt((ulong)compressedLength);

			_innerStream.Write(compressed, 0, compressedLength);

			_bufferOffset = 0;
		}

		/// <summary>Reads the next chunk from stream.</summary>
		/// <returns><c>true</c> if next has been read, or <c>false</c> if it is legitimate end of file.
		/// Throws <see cref="EndOfStreamException"/> if end of stream was unexpected.</returns>
		private bool AcquireNextChunk()
		{
			do
			{
				ulong varint;
				if (!TryReadVarInt(out varint)) return false;
				var flags = (ChunkFlags)varint;
				var isCompressed = (flags & ChunkFlags.Compressed) != 0;

				var originalLength = (int)ReadVarInt();
				var compressedLength = isCompressed ? (int)ReadVarInt() : originalLength;
				if (compressedLength > originalLength) throw EndOfStream(); // corrupted

				var compressed = new byte[compressedLength];
				var chunk = ReadBlock(compressed, 0, compressedLength);

				if (chunk != compressedLength) throw EndOfStream(); // corrupted

				if (!isCompressed)
				{
					_buffer = compressed; // no compression on this chunk
					_bufferLength = compressedLength;
				}
				else
				{
					if (_buffer == null || _buffer.Length < originalLength)
						_buffer = new byte[originalLength];
					var passes = (int)flags >> 2;
					if (passes != 0)
						throw new NotSupportedException("Chunks with multiple passes are not supported.");
					LZ4Codec.Decode(compressed, 0, compressedLength, _buffer, 0, originalLength, true);
					_bufferLength = originalLength;
				}

				_bufferOffset = 0;
			} while (_bufferLength == 0); // skip empty block (shouldn't happen but...)

			return true;
		}

		#endregion

		#region overrides

		/// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports reading.</summary>
		/// <returns>true if the stream supports reading; otherwise, false.</returns>
		public override bool CanRead
		{
			get { return _compressionMode == LZ4StreamMode.Decompress; }
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.</summary>
		/// <returns>true if the stream supports seeking; otherwise, false.</returns>
		public override bool CanSeek
		{
			get { return false; }
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports writing.</summary>
		/// <returns>true if the stream supports writing; otherwise, false.</returns>
		public override bool CanWrite
		{
			get { return _compressionMode == LZ4StreamMode.Compress; }
		}

		/// <summary>When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
		public override void Flush()
		{
			if (_bufferOffset > 0 && CanWrite) FlushCurrentChunk();
		}

		/// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
		/// <returns>A long value representing the length of the stream in bytes.</returns>
		public override long Length
		{
			get { return -1; }
		}

		/// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
		/// <returns>The current position within the stream.</returns>
		public override long Position
		{
			get { return -1; }
			set { throw NotSupported("SetPosition"); }
		}

		/// <summary>Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
		/// <returns>The unsigned byte cast to an Int32, or -1 if at the end of the stream.</returns>
		public override int ReadByte()
		{
			if (!CanRead) throw NotSupported("Read");

			if (_bufferOffset >= _bufferLength && !AcquireNextChunk())
				return -1; // that's just end of stream
			return _buffer[_bufferOffset++];
		}

		/// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!CanRead) throw NotSupported("Read");

			var total = 0;

			while (count > 0)
			{
				var chunk = Math.Min(count, _bufferLength - _bufferOffset);
				if (chunk > 0)
				{
					Buffer.BlockCopy(_buffer, _bufferOffset, buffer, offset, chunk);
					_bufferOffset += chunk;
					total += chunk;
					if (_interactiveRead) break;
					offset += chunk;
					count -= chunk;
				}
				else
				{
					if (!AcquireNextChunk()) break;
				}
			}

			return total;
		}

		/// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
		/// <returns>The new position within the current stream.</returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw NotSupported("Seek");
		}

		/// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		public override void SetLength(long value)
		{
			throw NotSupported("SetLength");
		}

		/// <summary>Writes a byte to the current position in the stream and advances the position within the stream by one byte.</summary>
		/// <param name="value">The byte to write to the stream.</param>
		public override void WriteByte(byte value)
		{
			if (!CanWrite) throw NotSupported("Write");

			if (_buffer == null)
			{
				_buffer = new byte[_blockSize];
				_bufferLength = _blockSize;
				_bufferOffset = 0;
			}

			if (_bufferOffset >= _bufferLength)
			{
				FlushCurrentChunk();
			}

			_buffer[_bufferOffset++] = value;
		}

		/// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!CanWrite) throw NotSupported("Write");

			if (_buffer == null)
			{
				_buffer = new byte[_blockSize];
				_bufferLength = _blockSize;
				_bufferOffset = 0;
			}

			while (count > 0)
			{
				var chunk = Math.Min(count, _bufferLength - _bufferOffset);
				if (chunk > 0)
				{
					Buffer.BlockCopy(buffer, offset, _buffer, _bufferOffset, chunk);
					offset += chunk;
					count -= chunk;
					_bufferOffset += chunk;
				}
				else
				{
					FlushCurrentChunk();
				}
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			Flush();
			if (!_isolateInnerStream)
				_innerStream.Dispose();
			base.Dispose(disposing);
		}

		#endregion
	}
}
}
