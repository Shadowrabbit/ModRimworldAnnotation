using System;

namespace Ionic.Zlib
{
	// Token: 0x0200183F RID: 6207
	public static class ZlibConstants
	{
		// Token: 0x04005C4E RID: 23630
		public const int WindowBitsMax = 15;

		// Token: 0x04005C4F RID: 23631
		public const int WindowBitsDefault = 15;

		// Token: 0x04005C50 RID: 23632
		public const int Z_OK = 0;

		// Token: 0x04005C51 RID: 23633
		public const int Z_STREAM_END = 1;

		// Token: 0x04005C52 RID: 23634
		public const int Z_NEED_DICT = 2;

		// Token: 0x04005C53 RID: 23635
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x04005C54 RID: 23636
		public const int Z_DATA_ERROR = -3;

		// Token: 0x04005C55 RID: 23637
		public const int Z_BUF_ERROR = -5;

		// Token: 0x04005C56 RID: 23638
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x04005C57 RID: 23639
		public const int WorkingBufferSizeMin = 1024;
	}
}
