using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001273 RID: 4723
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x06006700 RID: 26368 RVA: 0x00046657 File Offset: 0x00044857
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		// Token: 0x06006701 RID: 26369 RVA: 0x00046672 File Offset: 0x00044872
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06006702 RID: 26370 RVA: 0x001FAC84 File Offset: 0x001F8E84
		protected override void TakePrintFrom(Thing t)
		{
			if (t.Faction != null && t.Faction != Faction.OfPlayer)
			{
				return;
			}
			Building building = t as Building;
			if (building != null)
			{
				building.PrintForPowerGrid(this);
			}
		}
	}
}
