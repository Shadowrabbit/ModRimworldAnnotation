using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199A RID: 6554
	public class Designator_Deconstruct : Designator
	{
		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x060090DB RID: 37083 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x060090DC RID: 37084 RVA: 0x0003283A File Offset: 0x00030A3A
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x060090DD RID: 37085 RVA: 0x0029B5F4 File Offset: 0x002997F4
		public Designator_Deconstruct()
		{
			this.defaultLabel = "DesignatorDeconstruct".Translate();
			this.defaultDesc = "DesignatorDeconstructDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Deconstruct", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
		}

		// Token: 0x060090DE RID: 37086 RVA: 0x0029B678 File Offset: 0x00299878
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				return false;
			}
			AcceptanceReport result;
			if (this.TopDeconstructibleInCell(c, out result) == null)
			{
				return result;
			}
			return true;
		}

		// Token: 0x060090DF RID: 37087 RVA: 0x0029B6CC File Offset: 0x002998CC
		public override void DesignateSingleCell(IntVec3 loc)
		{
			AcceptanceReport acceptanceReport;
			this.DesignateThing(this.TopDeconstructibleInCell(loc, out acceptanceReport));
		}

		// Token: 0x060090E0 RID: 37088 RVA: 0x0029B6E8 File Offset: 0x002998E8
		private Thing TopDeconstructibleInCell(IntVec3 loc, out AcceptanceReport reportToDisplay)
		{
			reportToDisplay = AcceptanceReport.WasRejected;
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				AcceptanceReport acceptanceReport = this.CanDesignateThing(thing);
				if (this.CanDesignateThing(thing).Accepted)
				{
					reportToDisplay = AcceptanceReport.WasAccepted;
					return thing;
				}
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					reportToDisplay = acceptanceReport;
				}
			}
			return null;
		}

		// Token: 0x060090E1 RID: 37089 RVA: 0x0029B7AC File Offset: 0x002999AC
		public override void DesignateThing(Thing t)
		{
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (DebugSettings.godMode || innerIfMinified.GetStatValue(StatDefOf.WorkToBuild, true) == 0f || t.def.IsFrame)
			{
				t.Destroy(DestroyMode.Deconstruct);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x060090E2 RID: 37090 RVA: 0x0029B810 File Offset: 0x00299A10
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t.GetInnerIfMinified() as Building;
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			if (!building.DeconstructibleBy(Faction.OfPlayer))
			{
				if (building.Faction == Faction.OfMechanoids && building.def.building.IsDeconstructible)
				{
					return new AcceptanceReport("MessageMustDesignateDeconstructibleMechCluster".Translate());
				}
				return false;
			}
			else
			{
				if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
				{
					return false;
				}
				if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
				{
					return false;
				}
				return true;
			}
		}

		// Token: 0x060090E3 RID: 37091 RVA: 0x0006127F File Offset: 0x0005F47F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
