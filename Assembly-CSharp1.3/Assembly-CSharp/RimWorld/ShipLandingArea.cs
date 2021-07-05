using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001194 RID: 4500
	public class ShipLandingArea
	{
		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06006C3C RID: 27708 RVA: 0x00244664 File Offset: 0x00242864
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06006C3D RID: 27709 RVA: 0x00244671 File Offset: 0x00242871
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06006C3E RID: 27710 RVA: 0x00244679 File Offset: 0x00242879
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof;
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06006C3F RID: 27711 RVA: 0x0024468E File Offset: 0x0024288E
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06006C40 RID: 27712 RVA: 0x00244696 File Offset: 0x00242896
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06006C41 RID: 27713 RVA: 0x002446A0 File Offset: 0x002428A0
		public bool Active
		{
			get
			{
				for (int i = 0; i < this.beacons.Count; i++)
				{
					if (!this.beacons[i].Active)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x002446D9 File Offset: 0x002428D9
		public ShipLandingArea(CellRect rect, Map map)
		{
			this.rect = rect;
			this.map = map;
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x002446FC File Offset: 0x002428FC
		public void RecalculateBlockingThing()
		{
			this.blockedByRoof = false;
			foreach (IntVec3 c in this.rect)
			{
				if (c.Roofed(this.map))
				{
					this.blockedByRoof = true;
					break;
				}
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!(thingList[i] is Pawn) && (thingList[i].def.Fillage != FillCategory.None || thingList[i].def.IsEdifice() || thingList[i] is Skyfaller))
					{
						this.firstBlockingThing = thingList[i];
						return;
					}
				}
			}
			this.firstBlockingThing = null;
		}

		// Token: 0x04003C26 RID: 15398
		private CellRect rect;

		// Token: 0x04003C27 RID: 15399
		private Map map;

		// Token: 0x04003C28 RID: 15400
		private Thing firstBlockingThing;

		// Token: 0x04003C29 RID: 15401
		private bool blockedByRoof;

		// Token: 0x04003C2A RID: 15402
		public List<CompShipLandingBeacon> beacons = new List<CompShipLandingBeacon>();
	}
}
