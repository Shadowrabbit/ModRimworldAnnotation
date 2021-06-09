using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D5 RID: 6613
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x0600922C RID: 37420 RVA: 0x0029FAF4 File Offset: 0x0029DCF4
		public Designator_PlantsHarvestWood()
		{
			this.defaultLabel = "DesignatorHarvestWood".Translate();
			this.defaultDesc = "DesignatorHarvestWoodDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Harvest;
			this.hotKey = KeyBindingDefOf.Misc1;
			this.designationDef = DesignationDefOf.HarvestPlant;
			this.tutorTag = "PlantsHarvestWood";
		}

		// Token: 0x0600922D RID: 37421 RVA: 0x0029FB8C File Offset: 0x0029DD8C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || !plant.def.plant.IsTree)
			{
				return "MessageMustDesignateHarvestableWood".Translate();
			}
			return true;
		}

		// Token: 0x0600922E RID: 37422 RVA: 0x00061F00 File Offset: 0x00060100
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.IsTree;
		}

		// Token: 0x0600922F RID: 37423 RVA: 0x00061EA8 File Offset: 0x000600A8
		public override void DesignateThing(Thing t)
		{
			if (t.def == ThingDefOf.Plant_TreeAnima)
			{
				Messages.Message("MessageWarningCutAnimaTree".Translate(), t, MessageTypeDefOf.CautionInput, false);
			}
			base.DesignateThing(t);
		}
	}
}
