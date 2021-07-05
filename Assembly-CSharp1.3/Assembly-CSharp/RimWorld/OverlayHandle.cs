using System;

namespace RimWorld
{
	// Token: 0x0200104B RID: 4171
	public struct OverlayHandle
	{
		// Token: 0x0600629A RID: 25242 RVA: 0x00216CBE File Offset: 0x00214EBE
		public OverlayHandle(ThingOverlaysHandle thingOverlayHandle, OverlayTypes overlayType, int handleId)
		{
			this.thingOverlayHandle = thingOverlayHandle;
			this.overlayType = overlayType;
			this.handleId = handleId;
		}

		// Token: 0x040037FA RID: 14330
		public readonly ThingOverlaysHandle thingOverlayHandle;

		// Token: 0x040037FB RID: 14331
		public readonly OverlayTypes overlayType;

		// Token: 0x040037FC RID: 14332
		public int handleId;
	}
}
