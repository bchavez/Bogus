namespace Bogus.Compression {
#region LZ4 original

/*
   LZ4 - Fast LZ compression algorithm
   Copyright (C) 2011-2012, Yann Collet.
   BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are
   met:

	   * Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.
	   * Redistributions in binary form must reproduce the above
   copyright notice, this list of conditions and the following disclaimer
   in the documentation and/or other materials provided with the
   distribution.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
   OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
   LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

   You can contact the author at :
   - LZ4 homepage : http://fastcompression.blogspot.com/p/lz4.html
   - LZ4 source repository : http://code.google.com/p/lz4/
*/

#endregion

#region LZ4 port

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

// ReSharper disable InconsistentNaming

namespace LZ4ps
{
	public static partial class LZ4Codec
	{
		// Update chains up to ip (excluded)
		private static void LZ4HC_Insert_32(LZ4HC_Data_Structure ctx, int src_p)
		{
			var chainTable = ctx.chainTable;
			var hashTable = ctx.hashTable;
			var nextToUpdate = ctx.nextToUpdate;
			var src = ctx.src;
			var src_base = ctx.src_base;

			while (nextToUpdate < src_p)
			{
				var p = nextToUpdate;
				var delta = (p) - (hashTable[(((Peek4(src, p)) * 2654435761u) >> HASHHC_ADJUST)] + src_base);
				if (delta > MAX_DISTANCE) delta = MAX_DISTANCE;
				chainTable[(p) & MAXD_MASK] = (ushort)delta;
				hashTable[(((Peek4(src, p)) * 2654435761u) >> HASHHC_ADJUST)] = ((p) - src_base);
				nextToUpdate++;
			}

			ctx.nextToUpdate = nextToUpdate;
		}

		private static int LZ4HC_CommonLength_32(LZ4HC_Data_Structure ctx, int p1, int p2)
		{
			var debruijn32 = DEBRUIJN_TABLE_32;
			var src = ctx.src;
			var src_LASTLITERALS = ctx.src_LASTLITERALS;

			var p1t = p1;

			while (p1t < src_LASTLITERALS - (STEPSIZE_32 - 1))
			{
				var diff = (int)Xor4(src, p2, p1t);
				if (diff == 0)
				{
					p1t += STEPSIZE_32;
					p2 += STEPSIZE_32;
					continue;
				}
				p1t += debruijn32[((uint)((diff) & -(diff)) * 0x077CB531u) >> 27];
				return (p1t - p1);
			}
			if ((p1t < (src_LASTLITERALS - 1)) && (Equal2(src, p2, p1t)))
			{
				p1t += 2;
				p2 += 2;
			}
			if ((p1t < src_LASTLITERALS) && (src[p2] == src[p1t])) p1t++;
			return (p1t - p1);
		}

		private static int LZ4HC_InsertAndFindBestMatch_32(LZ4HC_Data_Structure ctx, int src_p, ref int src_match)
		{
			var chainTable = ctx.chainTable;
			var hashTable = ctx.hashTable;
			var src = ctx.src;
			var src_base = ctx.src_base;

			var nbAttempts = MAX_NB_ATTEMPTS;
			int repl = 0, ml = 0;
			ushort delta = 0;

			// HC4 match finder
			LZ4HC_Insert_32(ctx, src_p);
			var src_ref = (hashTable[(((Peek4(src, src_p)) * 2654435761u) >> HASHHC_ADJUST)] + src_base);


			// Detect repetitive sequences of length <= 4
			if (src_ref >= src_p - 4) // potential repetition
			{
				if (Equal4(src, src_ref, src_p)) // confirmed
				{
					delta = (ushort)(src_p - src_ref);
					repl = ml = LZ4HC_CommonLength_32(ctx, src_p + MINMATCH, src_ref + MINMATCH) + MINMATCH;
					src_match = src_ref;
				}
				src_ref = ((src_ref) - chainTable[(src_ref) & MAXD_MASK]);
			}

			while ((src_ref >= src_p - MAX_DISTANCE) && (nbAttempts != 0))
			{
				nbAttempts--;
				if (src[(src_ref + ml)] == src[(src_p + ml)])
				{
					if (Equal4(src, src_ref, src_p))
					{
						var mlt = LZ4HC_CommonLength_32(ctx, src_p + MINMATCH, src_ref + MINMATCH) + MINMATCH;
						if (mlt > ml)
						{
							ml = mlt;
							src_match = src_ref;
						}
					}
				}
				src_ref = ((src_ref) - chainTable[(src_ref) & MAXD_MASK]);
			}


			// Complete table
			if (repl != 0)
			{
				var src_ptr = src_p;

				var end = src_p + repl - (MINMATCH - 1);
				while (src_ptr < end - delta)
				{
					chainTable[(src_ptr) & MAXD_MASK] = delta; // Pre-Load
					src_ptr++;
				}
				do
				{
					chainTable[(src_ptr) & MAXD_MASK] = delta;
					hashTable[(((Peek4(src, src_ptr)) * 2654435761u) >> HASHHC_ADJUST)] = ((src_ptr) - src_base); // Head of chain
					src_ptr++;
				} while (src_ptr < end);
				ctx.nextToUpdate = end;
			}

			return ml;
		}

		private static int LZ4HC_InsertAndGetWiderMatch_32(
			LZ4HC_Data_Structure ctx, int src_p, int startLimit, int longest, ref int matchpos, ref int startpos)
		{
			var chainTable = ctx.chainTable;
			var hashTable = ctx.hashTable;
			var src = ctx.src;
			var src_base = ctx.src_base;
			var src_LASTLITERALS = ctx.src_LASTLITERALS;
			var debruijn32 = DEBRUIJN_TABLE_32;
			var nbAttempts = MAX_NB_ATTEMPTS;
			var delta = (src_p - startLimit);

			// First Match
			LZ4HC_Insert_32(ctx, src_p);
			var src_ref = (hashTable[(((Peek4(src, src_p)) * 2654435761u) >> HASHHC_ADJUST)] + src_base);

			while ((src_ref >= src_p - MAX_DISTANCE) && (nbAttempts != 0))
			{
				nbAttempts--;
				if (src[(startLimit + longest)] == src[(src_ref - delta + longest)])
				{
					if (Equal4(src, src_ref, src_p))
					{
						var reft = src_ref + MINMATCH;
						var ipt = src_p + MINMATCH;
						var startt = src_p;

						while (ipt < src_LASTLITERALS - (STEPSIZE_32 - 1))
						{
							var diff = (int)Xor4(src, reft, ipt);
							if (diff == 0)
							{
								ipt += STEPSIZE_32;
								reft += STEPSIZE_32;
								continue;
							}
							ipt += debruijn32[((uint)((diff) & -(diff)) * 0x077CB531u) >> 27];
							goto _endCount;
						}
						if ((ipt < (src_LASTLITERALS - 1)) && (Equal2(src, reft, ipt)))
						{
							ipt += 2;
							reft += 2;
						}
						if ((ipt < src_LASTLITERALS) && (src[reft] == src[ipt])) ipt++;

					_endCount:
						reft = src_ref;

						while ((startt > startLimit) && (reft > src_base) && (src[startt - 1] == src[reft - 1]))
						{
							startt--;
							reft--;
						}

						if ((ipt - startt) > longest)
						{
							longest = (ipt - startt);
							matchpos = reft;
							startpos = startt;
						}
					}
				}
				src_ref = ((src_ref) - chainTable[(src_ref) & MAXD_MASK]);
			}

			return longest;
		}

		private static int LZ4_encodeSequence_32(
			LZ4HC_Data_Structure ctx, ref int src_p, ref int dst_p, ref int src_anchor, int matchLength, int src_ref, int dst_end)
		{
			int len;
			var src = ctx.src;
			var dst = ctx.dst;

			// Encode Literal length
			var length = src_p - src_anchor;
			var dst_token = dst_p++;
			if ((dst_p + length + (2 + 1 + LASTLITERALS) + (length >> 8)) > dst_end) return 1; // Check output limit
			if (length >= RUN_MASK)
			{
				dst[dst_token] = (RUN_MASK << ML_BITS);
				len = length - RUN_MASK;
				for (; len > 254; len -= 255) dst[dst_p++] = 255;
				dst[dst_p++] = (byte)len;
			}
			else
			{
				dst[dst_token] = (byte)(length << ML_BITS);
			}

			// Copy Literals
			if (length > 0)
			{
				var _i = dst_p + length;
				src_anchor += WildCopy(src, src_anchor, dst, dst_p, _i);
				dst_p = _i;
			}

			// Encode Offset
			Poke2(dst, dst_p, (ushort)(src_p - src_ref));
			dst_p += 2;

			// Encode MatchLength
			len = (matchLength - MINMATCH);
			if (dst_p + (1 + LASTLITERALS) + (length >> 8) > dst_end) return 1; // Check output limit
			if (len >= ML_MASK)
			{
				dst[dst_token] += ML_MASK;
				len -= ML_MASK;
				for (; len > 509; len -= 510)
				{
					dst[(dst_p)++] = 255;
					dst[(dst_p)++] = 255;
				}
				if (len > 254)
				{
					len -= 255;
					dst[(dst_p)++] = 255;
				}
				dst[(dst_p)++] = (byte)len;
			}
			else
			{
				dst[dst_token] += (byte)len;
			}

			// Prepare next loop
			src_p += matchLength;
			src_anchor = src_p;

			return 0;
		}

		private static int LZ4_compressHCCtx_32(LZ4HC_Data_Structure ctx)
		{
			var src = ctx.src;
			var dst = ctx.dst;
			var src_0 = ctx.src_base;
			var src_end = ctx.src_end;
			var dst_0 = ctx.dst_base;
			var dst_len = ctx.dst_len;
			var dst_end = ctx.dst_end;

			var src_p = src_0;
			var src_anchor = src_p;
			var src_mflimit = src_end - MFLIMIT;

			var dst_p = dst_0;

			var src_ref = 0;
			var start2 = 0;
			var ref2 = 0;
			var start3 = 0;
			var ref3 = 0;

			src_p++;

			// Main Loop
			while (src_p < src_mflimit)
			{
				var ml = LZ4HC_InsertAndFindBestMatch_32(ctx, src_p, ref src_ref);
				if (ml == 0)
				{
					src_p++;
					continue;
				}

				// saved, in case we would skip too much
				var start0 = src_p;
				var ref0 = src_ref;
				var ml0 = ml;

			_Search2:
				var ml2 = src_p + ml < src_mflimit
					? LZ4HC_InsertAndGetWiderMatch_32(ctx, src_p + ml - 2, src_p + 1, ml, ref ref2, ref start2)
					: ml;

				if (ml2 == ml) // No better match
				{
					if (LZ4_encodeSequence_32(ctx, ref src_p, ref dst_p, ref src_anchor, ml, src_ref, dst_end) != 0) return 0;
					continue;
				}

				if (start0 < src_p)
				{
					if (start2 < src_p + ml0) // empirical
					{
						src_p = start0;
						src_ref = ref0;
						ml = ml0;
					}
				}

				// Here, start0==ip
				if ((start2 - src_p) < 3) // First Match too small : removed
				{
					ml = ml2;
					src_p = start2;
					src_ref = ref2;
					goto _Search2;
				}

			_Search3:
				// Currently we have :
				// ml2 > ml1, and
				// ip1+3 <= ip2 (usually < ip1+ml1)
				if ((start2 - src_p) < OPTIMAL_ML)
				{
					var new_ml = ml;
					if (new_ml > OPTIMAL_ML) new_ml = OPTIMAL_ML;
					if (src_p + new_ml > start2 + ml2 - MINMATCH) new_ml = (start2 - src_p) + ml2 - MINMATCH;
					var correction = new_ml - (start2 - src_p);
					if (correction > 0)
					{
						start2 += correction;
						ref2 += correction;
						ml2 -= correction;
					}
				}
				// Now, we have start2 = ip+new_ml, with new_ml=min(ml, OPTIMAL_ML=18)

				var ml3 = start2 + ml2 < src_mflimit
					? LZ4HC_InsertAndGetWiderMatch_32(ctx, start2 + ml2 - 3, start2, ml2, ref ref3, ref start3)
					: ml2;

				if (ml3 == ml2) // No better match : 2 sequences to encode
				{
					// ip & ref are known; Now for ml
					if (start2 < src_p + ml) ml = (start2 - src_p);
					// Now, encode 2 sequences
					if (LZ4_encodeSequence_32(ctx, ref src_p, ref dst_p, ref src_anchor, ml, src_ref, dst_end) != 0) return 0;
					src_p = start2;
					if (LZ4_encodeSequence_32(ctx, ref src_p, ref dst_p, ref src_anchor, ml2, ref2, dst_end) != 0) return 0;
					continue;
				}

				if (start3 < src_p + ml + 3) // Not enough space for match 2 : remove it
				{
					if (start3 >= (src_p + ml)) // can write Seq1 immediately ==> Seq2 is removed, so Seq3 becomes Seq1
					{
						if (start2 < src_p + ml)
						{
							var correction = (src_p + ml - start2);
							start2 += correction;
							ref2 += correction;
							ml2 -= correction;
							if (ml2 < MINMATCH)
							{
								start2 = start3;
								ref2 = ref3;
								ml2 = ml3;
							}
						}

						if (LZ4_encodeSequence_32(ctx, ref src_p, ref dst_p, ref src_anchor, ml, src_ref, dst_end) != 0) return 0;
						src_p = start3;
						src_ref = ref3;
						ml = ml3;

						start0 = start2;
						ref0 = ref2;
						ml0 = ml2;
						goto _Search2;
					}

					start2 = start3;
					ref2 = ref3;
					ml2 = ml3;
					goto _Search3;
				}

				// OK, now we have 3 ascending matches; let's write at least the first one
				// ip & ref are known; Now for ml
				if (start2 < src_p + ml)
				{
					if ((start2 - src_p) < ML_MASK)
					{
						if (ml > OPTIMAL_ML) ml = OPTIMAL_ML;
						if (src_p + ml > start2 + ml2 - MINMATCH) ml = (start2 - src_p) + ml2 - MINMATCH;
						var correction = ml - (start2 - src_p);
						if (correction > 0)
						{
							start2 += correction;
							ref2 += correction;
							ml2 -= correction;
						}
					}
					else
					{
						ml = (start2 - src_p);
					}
				}
				if (LZ4_encodeSequence_32(ctx, ref src_p, ref dst_p, ref src_anchor, ml, src_ref, dst_end) != 0) return 0;

				src_p = start2;
				src_ref = ref2;
				ml = ml2;

				start2 = start3;
				ref2 = ref3;
				ml2 = ml3;

				goto _Search3;
			}

			// Encode Last Literals
			{
				var lastRun = (src_end - src_anchor);
				if ((dst_p - dst_0) + lastRun + 1 + ((lastRun + 255 - RUN_MASK) / 255) > (uint)dst_len) return 0; // Check output limit
				if (lastRun >= RUN_MASK)
				{
					dst[dst_p++] = (RUN_MASK << ML_BITS);
					lastRun -= RUN_MASK;
					for (; lastRun > 254; lastRun -= 255) dst[dst_p++] = 255;
					dst[dst_p++] = (byte)lastRun;
				}
				else
				{
					dst[dst_p++] = (byte)(lastRun << ML_BITS);
				}
				BlockCopy(src, src_anchor, dst, dst_p, src_end - src_anchor);
				dst_p += src_end - src_anchor;
			}

			// End
			return (dst_p - dst_0);
		}
	}
}

// ReSharper restore InconsistentNaming
}
