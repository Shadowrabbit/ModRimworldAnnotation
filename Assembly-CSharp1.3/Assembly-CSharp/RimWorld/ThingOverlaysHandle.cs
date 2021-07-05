using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104C RID: 4172
	public class ThingOverlaysHandle
	{
		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x0600629B RID: 25243 RVA: 0x00216CD8 File Offset: 0x00214ED8
		public OverlayTypes OverlayTypes
		{
			get
			{
				if (this.overlayTypesDirty)
				{
					this.overlayTypes = OverlayTypes.None;
					for (int i = 0; i < this.overlayHandles.Count; i++)
					{
						this.overlayTypes |= this.overlayHandles[i].overlayType;
					}
					this.overlayTypesDirty = false;
				}
				return this.overlayTypes;
			}
		}

		// Token: 0x0600629C RID: 25244 RVA: 0x00216D35 File Offset: 0x00214F35
		public ThingOverlaysHandle(OverlayDrawer drawer, Thing thing)
		{
			this.drawer = drawer;
			this.thing = thing;
		}

		// Token: 0x0600629D RID: 25245 RVA: 0x00216D58 File Offset: 0x00214F58
		public OverlayHandle Enable(OverlayTypes overlayType)
		{
			int num = this.handleIdCounter;
			this.handleIdCounter = num + 1;
			OverlayHandle overlayHandle = new OverlayHandle(this, overlayType, num);
			this.overlayHandles.Add(overlayHandle);
			this.overlayTypesDirty = true;
			return overlayHandle;
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x00216D94 File Offset: 0x00214F94
		public void Disable(OverlayHandle handle)
		{
			for (int i = this.overlayHandles.Count - 1; i >= 0; i--)
			{
				if (this.overlayHandles[i].handleId == handle.handleId)
				{
					this.overlayHandles.RemoveAt(i);
				}
			}
			this.overlayTypesDirty = true;
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x00216DE5 File Offset: 0x00214FE5
		public void Disable(ref OverlayHandle? handle)
		{
			if (handle != null)
			{
				this.Disable(handle.Value);
				handle = null;
			}
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x00216E02 File Offset: 0x00215002
		public void Dispose()
		{
			if (this.disposed)
			{
				Log.Error("Tried disposing already disposed ThingOverlaysHandle!");
				return;
			}
			this.disposed = true;
		}

		// Token: 0x040037FD RID: 14333
		public readonly OverlayDrawer drawer;

		// Token: 0x040037FE RID: 14334
		public readonly Thing thing;

		// Token: 0x040037FF RID: 14335
		private bool disposed;

		// Token: 0x04003800 RID: 14336
		private int handleIdCounter;

		// Token: 0x04003801 RID: 14337
		private bool overlayTypesDirty;

		// Token: 0x04003802 RID: 14338
		private OverlayTypes overlayTypes;

		// Token: 0x04003803 RID: 14339
		private List<OverlayHandle> overlayHandles = new List<OverlayHandle>();
	}
}
