namespace Bogus.Compression {
using System;

namespace LZ4
{
	/// <summary>
	/// Additional flags for LZ4Stream.
	/// </summary>
	[Flags]
	public enum LZ4StreamFlags
	{
		/// <summary>Empty settings. No special behavior.</summary>
		None = 0x00, 

		/// <summary>Allows to use interactive mode, possibly returning partial blocks.</summary>
		InteractiveRead = 0x01,

		/// <summary>Uses high compression version of algorithm.</summary>
		HighCompression = 0x02,

		/// <summary>Isolates inner stream so it does not get 
		/// closed when LZ4 stream is closed.</summary>
		IsolateInnerStream = 0x04,

		/// <summary>Default settings.</summary>
		Default = None,
	}
}
}
