using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D4 RID: 4820
	public class Designator_PlantsHarvest : Designator_Plants
	{
		// Token: 0x06007337 RID: 29495 RVA: 0x0026758C File Offset: 0x0026578C
		public Designator_PlantsHarvest()
		{
			this.defaultLabel = "DesignatorHarvest".Translate();
			this.defaultDesc = "DesignatorHarvestDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Harvest", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Harvest;
			this.hotKey = KeyBindingDefOf.Misc2;
			this.designationDef = DesignationDefOf.HarvestPlant;
		}

		// Token: 0x06007338 RID: 29496 RVA: 0x00267618 File Offset: 0x00265818
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || plant.def.plant.harvestTag != "Standard")
			{
				return "MessageMustDesignateHarvestable".Translate();
			}
			return true;
		}

		// Token: 0x06007339 RID: 29497 RVA: 0x00267679 File Offset: 0x00265879
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Standard";
		}
	}
}
