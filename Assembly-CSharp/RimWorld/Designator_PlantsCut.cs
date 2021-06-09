using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D3 RID: 6611
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x06009224 RID: 37412 RVA: 0x0029F90C File Offset: 0x0029DB0C
		public Designator_PlantsCut()
		{
			this.defaultLabel = "DesignatorCutPlants".Translate();
			this.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_CutPlants;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.designationDef = DesignationDefOf.CutPlant;
		}

		// Token: 0x06009225 RID: 37413 RVA: 0x0029F998 File Offset: 0x0029DB98
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			return this.AffectsThing(t);
		}

		// Token: 0x06009226 RID: 37414 RVA: 0x00061E99 File Offset: 0x00060099
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return this.AffectsThing(target.Thing);
		}

		// Token: 0x06009227 RID: 37415 RVA: 0x0029F9C4 File Offset: 0x0029DBC4
		private bool AffectsThing(Thing t)
		{
			Plant plant;
			return (plant = (t as Plant)) != null && (this.isOrder || !plant.def.plant.IsTree || !plant.HarvestableNow);
		}

		// Token: 0x06009228 RID: 37416 RVA: 0x00061EA8 File Offset: 0x000600A8
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
