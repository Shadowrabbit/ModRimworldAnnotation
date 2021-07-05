using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183B RID: 6203
	public class ShipLandingArea
	{
		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06008983 RID: 35203 RVA: 0x0005C5AB File Offset: 0x0005A7AB
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06008984 RID: 35204 RVA: 0x0005C5B8 File Offset: 0x0005A7B8
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06008985 RID: 35205 RVA: 0x0005C5C0 File Offset: 0x0005A7C0
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof;
			}
		}

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x06008986 RID: 35206 RVA: 0x0005C5D5 File Offset: 0x0005A7D5
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x06008987 RID: 35207 RVA: 0x0005C5DD File Offset: 0x0005A7DD
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x06008988 RID: 35208 RVA: 0x0028262C File Offset: 0x0028082C
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

		// Token: 0x06008989 RID: 35209 RVA: 0x0005C5E5 File Offset: 0x0005A7E5
		public ShipLandingArea(CellRect rect, Map map)
		{
			this.rect = rect;
			this.map = map;
		}

		// Token: 0x0600898A RID: 35210 RVA: 0x00282668 File Offset: 0x00280868
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

		// Token: 0x04005826 RID: 22566
		private CellRect rect;

		// Token: 0x04005827 RID: 22567
		private Map map;

		// Token: 0x04005828 RID: 22568
		private Thing firstBlockingThing;

		// Token: 0x04005829 RID: 22569
		private bool blockedByRoof;

		// Token: 0x0400582A RID: 22570
		public List<CompShipLandingBeacon> beacons = new List<CompShipLandingBeacon>();
	}
}
