using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D3 RID: 4819
	[StaticConstructorOnStartup]
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x06007331 RID: 29489 RVA: 0x00267404 File Offset: 0x00265604
		public Designator_PlantsCut()
		{
			this.defaultLabel = "DesignatorCutPlants".Translate();
			this.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			this.icon = Designator_PlantsCut.IconTex;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_CutPlants;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.designationDef = DesignationDefOf.CutPlant;
		}

		// Token: 0x06007332 RID: 29490 RVA: 0x0026748C File Offset: 0x0026568C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			return this.AffectsThing(t);
		}

		// Token: 0x06007333 RID: 29491 RVA: 0x002674B8 File Offset: 0x002656B8
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return this.AffectsThing(target.Thing);
		}

		// Token: 0x06007334 RID: 29492 RVA: 0x002674C8 File Offset: 0x002656C8
		private bool AffectsThing(Thing t)
		{
			Plant plant;
			return (plant = (t as Plant)) != null && (this.isOrder || !plant.def.plant.IsTree || !plant.HarvestableNow);
		}

		// Token: 0x06007335 RID: 29493 RVA: 0x00267508 File Offset: 0x00265708
		public override void DesignateThing(Thing t)
		{
			if (t.def == ThingDefOf.Plant_TreeAnima)
			{
				Messages.Message("MessageWarningCutAnimaTree".Translate(), t, MessageTypeDefOf.CautionInput, false);
			}
			if (ModsConfig.IdeologyActive && t.def.plant.IsTree && t.def.plant.treeLoversCareIfChopped)
			{
				Designator_PlantsHarvestWood.PossiblyWarnPlayerOnDesignatingTreeCut();
			}
			base.DesignateThing(t);
		}

		// Token: 0x04003ED8 RID: 16088
		public static Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
	}
}
