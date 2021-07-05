using System;

namespace Ionic.Zlib
{
	// Token: 0x02001830 RID: 6192
	internal class WorkItem
	{
		// Token: 0x060091AA RID: 37290 RVA: 0x00346624 File Offset: 0x00344824
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

		// Token: 0x04005BC6 RID: 23494
		public byte[] buffer;

		// Token: 0x04005BC7 RID: 23495
		public byte[] compressed;

		// Token: 0x04005BC8 RID: 23496
		public int crc;

		// Token: 0x04005BC9 RID: 23497
		public int index;

		// Token: 0x04005BCA RID: 23498
		public int ordinal;

		// Token: 0x04005BCB RID: 23499
		public int inputBytesAvailable;

		// Token: 0x04005BCC RID: 23500
		public int compressedBytesAvailable;

		// Token: 0x04005BCD RID: 23501
		public ZlibCodec compressor;
	}
}
