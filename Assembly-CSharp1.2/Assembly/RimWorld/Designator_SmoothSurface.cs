using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B3 RID: 6579
	public class Designator_SmoothSurface : Designator
	{
		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x0600916F RID: 37231 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x06009170 RID: 37232 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009171 RID: 37233 RVA: 0x0029CC28 File Offset: 0x0029AE28
		public Designator_SmoothSurface()
		{
			this.defaultLabel = "DesignatorSmoothSurface".Translate();
			this.defaultDesc = "DesignatorSmoothSurfaceDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_SmoothSurface;
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06009172 RID: 37234 RVA: 0x0029CCAC File Offset: 0x0029AEAC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t != null && t.def.IsSmoothable && this.CanDesignateCell(t.Position).Accepted)
			{
				return AcceptanceReport.WasAccepted;
			}
			return false;
		}

		// Token: 0x06009173 RID: 37235 RVA: 0x00061586 File Offset: 0x0005F786
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x06009174 RID: 37236 RVA: 0x0029CCEC File Offset: 0x0029AEEC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor) != null || base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (edifice != null && !SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(edifice))
			{
				return "MessageMustDesignateSmoothableSurface".Translate();
			}
			if (!c.GetTerrain(base.Map).affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))
			{
				return "MessageMustDesignateSmoothableSurface".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009175 RID: 37237 RVA: 0x0029CDF4 File Offset: 0x0029AFF4
		public override void DesignateSingleCell(IntVec3 c)
		{
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothWall));
				base.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.Mine);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothFloor));
		}

		// Token: 0x06009176 RID: 37238 RVA: 0x0006127F File Offset: 0x0005F47F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06009177 RID: 37239 RVA: 0x00060FB9 File Offset: 0x0005F1B9
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
