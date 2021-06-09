using System;

namespace Ionic.Zlib
{
	// Token: 0x02002224 RID: 8740
	public static class ZlibConstants
	{
		// Token: 0x040080EE RID: 33006
		public const int WindowBitsMax = 15;

		// Token: 0x040080EF RID: 33007
		public const int WindowBitsDefault = 15;

		// Token: 0x040080F0 RID: 33008
		public const int Z_OK = 0;

		// Token: 0x040080F1 RID: 33009
		public const int Z_STREAM_END = 1;

		// Token: 0x040080F2 RID: 33010
		public const int Z_NEED_DICT = 2;

		// Token: 0x040080F3 RID: 33011
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x040080F4 RID: 33012
		public const int Z_DATA_ERROR = -3;

		// Token: 0x040080F5 RID: 33013
		public const int Z_BUF_ERROR = -5;

		// Token: 0x040080F6 RID: 33014
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x040080F7 RID: 33015
		public const int WorkingBufferSizeMin = 1024;
	}
}
