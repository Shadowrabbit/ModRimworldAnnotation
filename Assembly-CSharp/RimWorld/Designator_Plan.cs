using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AA RID: 6570
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x06009147 RID: 37191 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x06009148 RID: 37192 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x06009149 RID: 37193 RVA: 0x00061643 File Offset: 0x0005F843
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x0600914A RID: 37194 RVA: 0x0006164A File Offset: 0x0005F84A
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x0600914B RID: 37195 RVA: 0x0029C57C File Offset: 0x0029A77C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			if (this.mode == DesignateMode.Add)
			{
				if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
				{
					return false;
				}
			}
			else if (this.mode == DesignateMode.Remove && base.Map.designationManager.DesignationAt(c, this.Designation) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600914C RID: 37196 RVA: 0x0029C614 File Offset: 0x0029A814
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.designationManager.AddDesignation(new Designation(c, this.Designation));
				return;
			}
			if (this.mode == DesignateMode.Remove)
			{
				base.Map.designationManager.DesignationAt(c, this.Designation).Delete();
			}
		}

		// Token: 0x0600914D RID: 37197 RVA: 0x00061681 File Offset: 0x0005F881
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x0600914E RID: 37198 RVA: 0x00060FB9 File Offset: 0x0005F1B9
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x04005C2B RID: 23595
		private DesignateMode mode;
	}
}
