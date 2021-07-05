using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BC RID: 4796
	public class Designator_SmoothSurface : Designator
	{
		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06007290 RID: 29328 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06007291 RID: 29329 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x00263D6C File Offset: 0x00261F6C
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

		// Token: 0x06007293 RID: 29331 RVA: 0x00263DF0 File Offset: 0x00261FF0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t != null && t.def.IsSmoothable && this.CanDesignateCell(t.Position).Accepted)
			{
				return AcceptanceReport.WasAccepted;
			}
			return false;
		}

		// Token: 0x06007294 RID: 29332 RVA: 0x00262F6F File Offset: 0x0026116F
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x06007295 RID: 29333 RVA: 0x00263E30 File Offset: 0x00262030
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

		// Token: 0x06007296 RID: 29334 RVA: 0x00263F38 File Offset: 0x00262138
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

		// Token: 0x06007297 RID: 29335 RVA: 0x00261B2D File Offset: 0x0025FD2D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06007298 RID: 29336 RVA: 0x00260D19 File Offset: 0x0025EF19
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
