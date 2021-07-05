using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AA RID: 4778
	public class Designator_Deconstruct : Designator
	{
		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06007213 RID: 29203 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06007214 RID: 29204 RVA: 0x0011EE02 File Offset: 0x0011D002
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x06007215 RID: 29205 RVA: 0x00261EA8 File Offset: 0x002600A8
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

		// Token: 0x06007216 RID: 29206 RVA: 0x00261F2C File Offset: 0x0026012C
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

		// Token: 0x06007217 RID: 29207 RVA: 0x00261F80 File Offset: 0x00260180
		public override void DesignateSingleCell(IntVec3 loc)
		{
			AcceptanceReport acceptanceReport;
			this.DesignateThing(this.TopDeconstructibleInCell(loc, out acceptanceReport));
		}

		// Token: 0x06007218 RID: 29208 RVA: 0x00261F9C File Offset: 0x0026019C
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

		// Token: 0x06007219 RID: 29209 RVA: 0x00262060 File Offset: 0x00260260
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

		// Token: 0x0600721A RID: 29210 RVA: 0x002620C4 File Offset: 0x002602C4
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
				if (building.Faction != null && building.Faction == Faction.OfMechanoids && building.def.building.IsDeconstructible)
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

		// Token: 0x0600721B RID: 29211 RVA: 0x00261B2D File Offset: 0x0025FD2D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
