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
using System.IO.Compression;

namespace LZ4
{
	public partial class LZ4Stream
	{
		/// <summary>Initializes a new instance of the <see cref="LZ4Stream" /> class.</summary>
		/// <param name="innerStream">The inner stream.</param>
		/// <param name="compressionMode">The compression mode.</param>
		/// <param name="highCompression">if set to <c>true</c> [high compression].</param>
		/// <param name="blockSize">Size of the block.</param>
		/// <param name="interactiveRead">if set to <c>true</c> interactive read mode is used. 
		/// It means that <see cref="Read"/> method tries to return data as soon as possible. 
		/// Please note, that this should be default behavior but has been made optional for 
		/// backward compatibility. This constructor will be changed in next major release.</param>
		[Obsolete("This constructor is obsolete")]
		public LZ4Stream(
			Stream innerStream,
			CompressionMode compressionMode,
			bool highCompression,
			int blockSize = 1024*1024,
			bool interactiveRead = false)
			: this(
				innerStream,
				ToLZ4StreamMode(compressionMode),
				CombineLZ4Flags(highCompression, interactiveRead),
				blockSize)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="LZ4Stream" /> class.</summary>
		/// <param name="innerStream">The inner stream.</param>
		/// <param name="compressionMode">The compression mode.</param>
		/// <param name="compressionFlags">The compression flags.</param>
		/// <param name="blockSize">Size of the block.</param>
		public LZ4Stream(
			Stream innerStream,
			CompressionMode compressionMode,
			LZ4StreamFlags compressionFlags = LZ4StreamFlags.Default,
			int blockSize = 1024*1024)
			: this(
				innerStream,
				ToLZ4StreamMode(compressionMode),
				compressionFlags,
				blockSize)
		{
		}

		/// <summary>Converts CompressionMode to LZ4StreamMode.</summary>
		/// <param name="compressionMode">The compression mode.</param>
		/// <returns>LZ4StreamMode</returns>
		/// <exception cref="System.ArgumentException"></exception>
		private static LZ4StreamMode ToLZ4StreamMode(CompressionMode compressionMode)
		{
			switch (compressionMode)
			{
				case CompressionMode.Compress:
					return LZ4StreamMode.Compress;
				case CompressionMode.Decompress:
					return LZ4StreamMode.Decompress;
				default:
					throw new ArgumentException(
						string.Format("Unhandled compression mode: {0}", compressionMode));
			}
		}

		/// <summary>Combines the LZ4 flags.</summary>
		/// <param name="highCompression">if set to <c>true</c> high compression will be used.</param>
		/// <param name="interactiveRead">if set to <c>true</c> interactive read mode will be used.</param>
		/// <returns>LZ4 compression flags.</returns>
		private static LZ4StreamFlags CombineLZ4Flags(
			bool highCompression, bool interactiveRead)
		{
			var result = LZ4StreamFlags.Default;
			if (highCompression)
				result |= LZ4StreamFlags.HighCompression;
			if (interactiveRead)
				result |= LZ4StreamFlags.InteractiveRead;
			return result;
		}

	}
}
}
