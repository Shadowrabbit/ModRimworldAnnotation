using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B3 RID: 4787
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06007268 RID: 29288 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06007269 RID: 29289 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x0600726A RID: 29290 RVA: 0x002631F0 File Offset: 0x002613F0
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x0600726B RID: 29291 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool DragDrawOutline
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600726C RID: 29292 RVA: 0x002631F7 File Offset: 0x002613F7
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x0600726D RID: 29293 RVA: 0x00263230 File Offset: 0x00261430
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

		// Token: 0x0600726E RID: 29294 RVA: 0x002632C8 File Offset: 0x002614C8
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

		// Token: 0x0600726F RID: 29295 RVA: 0x00263324 File Offset: 0x00261524
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x06007270 RID: 29296 RVA: 0x00260D19 File Offset: 0x0025EF19
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x04003EC3 RID: 16067
		private DesignateMode mode;
	}
}
