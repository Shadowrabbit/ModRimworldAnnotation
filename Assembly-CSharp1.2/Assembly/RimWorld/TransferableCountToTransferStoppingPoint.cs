using System;

namespace RimWorld
{
	// Token: 0x02001A82 RID: 6786
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x060095DD RID: 38365 RVA: 0x0006400A File Offset: 0x0006220A
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x04005F67 RID: 24423
		public int threshold;

		// Token: 0x04005F68 RID: 24424
		public string leftLabel;

		// Token: 0x04005F69 RID: 24425
		public string rightLabel;
	}
}
