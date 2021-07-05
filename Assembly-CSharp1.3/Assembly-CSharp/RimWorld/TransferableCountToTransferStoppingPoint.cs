using System;

namespace RimWorld
{
	// Token: 0x02001318 RID: 4888
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x060075F6 RID: 30198 RVA: 0x0028A6D1 File Offset: 0x002888D1
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x04004155 RID: 16725
		public int threshold;

		// Token: 0x04004156 RID: 16726
		public string leftLabel;

		// Token: 0x04004157 RID: 16727
		public string rightLabel;
	}
}
