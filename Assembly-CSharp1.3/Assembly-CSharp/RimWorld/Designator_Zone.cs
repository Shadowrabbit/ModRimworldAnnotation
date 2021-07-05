using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C8 RID: 4808
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x06007306 RID: 29446 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x06007307 RID: 29447 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007308 RID: 29448 RVA: 0x0026693F File Offset: 0x00264B3F
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

		// Token: 0x06007309 RID: 29449 RVA: 0x00260D19 File Offset: 0x0025EF19
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
