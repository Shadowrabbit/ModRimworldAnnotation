using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C85 RID: 3205
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x06004AC9 RID: 19145 RVA: 0x0018B4F5 File Offset: 0x001896F5
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x0018B510 File Offset: 0x00189710
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x0018B520 File Offset: 0x00189720
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
