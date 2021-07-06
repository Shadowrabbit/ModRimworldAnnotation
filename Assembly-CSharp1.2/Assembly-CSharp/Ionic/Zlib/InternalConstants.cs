using System;

namespace Ionic.Zlib
{
	// Token: 0x0200221D RID: 8733
	internal static class InternalConstants
	{
		// Token: 0x040080AF RID: 32943
		internal static readonly int MAX_BITS = 15;

		// Token: 0x040080B0 RID: 32944
		internal static readonly int BL_CODES = 19;

		// Token: 0x040080B1 RID: 32945
		internal static readonly int D_CODES = 30;

		// Token: 0x040080B2 RID: 32946
		internal static readonly int LITERALS = 256;

		// Token: 0x040080B3 RID: 32947
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x040080B4 RID: 32948
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x040080B5 RID: 32949
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x040080B6 RID: 32950
		internal static readonly int REP_3_6 = 16;

		// Token: 0x040080B7 RID: 32951
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x040080B8 RID: 32952
		internal static readonly int REPZ_11_138 = 18;
	}
}
