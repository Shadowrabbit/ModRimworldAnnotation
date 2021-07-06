using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C8 RID: 6600
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x060091F9 RID: 37369 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x060091FA RID: 37370 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060091FB RID: 37371 RVA: 0x00061C4F File Offset: 0x0005FE4F
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			OverlayDrawHandler.DrawZonesThisFrame();
			if (Find.Selector.SelectedZone != null)
			{
				GenDraw.DrawFieldEdges(Find.Selector.SelectedZone.Cells);
			}
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x060091FC RID: 37372 RVA: 0x00060FB9 File Offset: 0x0005F1B9
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
