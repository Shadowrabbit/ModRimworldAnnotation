using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129E RID: 4766
	public class Designator_AreaBuildRoof : Designator_Area
	{
		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x060071DC RID: 29148 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x060071DD RID: 29149 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071DE RID: 29150 RVA: 0x00260F74 File Offset: 0x0025F174
		public Designator_AreaBuildRoof()
		{
			this.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc9;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.tutorTag = "AreaBuildRoofExpand";
		}

		// Token: 0x060071DF RID: 29151 RVA: 0x00260FFC File Offset: 0x0025F1FC
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
			return !base.Map.areaManager.BuildRoof[c];
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x00261051 File Offset: 0x0025F251
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = true;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x00261084 File Offset: 0x0025F284
		public override bool ShowWarningForCell(IntVec3 c)
		{
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
			{
				if (thing.def.plant != null && thing.def.plant.interferesWithRoof)
				{
					Messages.Message("MessageRoofIncompatibleWithPlant".Translate(thing), MessageTypeDefOf.CautionInput, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060071E2 RID: 29154 RVA: 0x0026111C File Offset: 0x0025F31C
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
