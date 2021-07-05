using System;

namespace Ionic.Zlib
{
	// Token: 0x0200220E RID: 8718
	internal static class InternalInflateConstants
	{
		// Token: 0x04008002 RID: 32770
		internal static readonly int[] InflateMask = new int[]
		{
			0,
			1,
			3,
			7,
			15,
			31,
			63,
			127,
			255,
			511,
			1023,
			2047,
			4095,
			8191,
			16383,
			32767,
			65535
		};
	}
}
