using System;

namespace Ionic.Zlib
{
	// Token: 0x02002213 RID: 8723
	internal class WorkItem
	{
		// Token: 0x0600BB62 RID: 47970 RVA: 0x003606D0 File Offset: 0x0035E8D0
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x0400804F RID: 32847
		public byte[] buffer;

		// Token: 0x04008050 RID: 32848
		public byte[] compressed;

		// Token: 0x04008051 RID: 32849
		public int crc;

		// Token: 0x04008052 RID: 32850
		public int index;

		// Token: 0x04008053 RID: 32851
		public int ordinal;

		// Token: 0x04008054 RID: 32852
		public int inputBytesAvailable;

		// Token: 0x04008055 RID: 32853
		public int compressedBytesAvailable;

		// Token: 0x04008056 RID: 32854
		public ZlibCodec compressor;
	}
}
