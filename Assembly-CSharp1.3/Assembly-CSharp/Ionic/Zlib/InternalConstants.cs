using System;

namespace Ionic.Zlib
{
	// Token: 0x02001839 RID: 6201
	internal static class InternalConstants
	{
		// Token: 0x04005C13 RID: 23571
		internal static readonly int MAX_BITS = 15;

		// Token: 0x04005C14 RID: 23572
		internal static readonly int BL_CODES = 19;

		// Token: 0x04005C15 RID: 23573
		internal static readonly int D_CODES = 30;

		// Token: 0x04005C16 RID: 23574
		internal static readonly int LITERALS = 256;

		// Token: 0x04005C17 RID: 23575
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x04005C18 RID: 23576
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x04005C19 RID: 23577
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x04005C1A RID: 23578
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04005C1B RID: 23579
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04005C1C RID: 23580
		internal static readonly int REPZ_11_138 = 18;
	}
}
