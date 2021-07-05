using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D5 RID: 4821
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x0600733A RID: 29498 RVA: 0x0026769C File Offset: 0x0026589C
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

		// Token: 0x0600733B RID: 29499 RVA: 0x00267734 File Offset: 0x00265934
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

		// Token: 0x0600733C RID: 29500 RVA: 0x0026778B File Offset: 0x0026598B
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.IsTree;
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x002677A4 File Offset: 0x002659A4
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

		// Token: 0x0600733E RID: 29502 RVA: 0x00267818 File Offset: 0x00265A18
		public static void PossiblyWarnPlayerOnDesignatingTreeCut()
		{
			Designator_PlantsHarvestWood.tmpIdeoMemberNames.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.WarnPlayerOnDesignateChopTree)
				{
					Designator_PlantsHarvestWood.tmpIdeoMemberNames.Add(Find.ActiveLanguageWorker.Pluralize(ideo.memberName, -1));
				}
			}
			if (Designator_PlantsHarvestWood.tmpIdeoMemberNames.Any<string>())
			{
				Messages.Message("MessageWarningPlayerDesignatedTreeChopped".Translate(Designator_PlantsHarvestWood.tmpIdeoMemberNames.ToCommaList(true, false)), MessageTypeDefOf.CautionInput, false);
			}
			Designator_PlantsHarvestWood.tmpIdeoMemberNames.Clear();
		}

		// Token: 0x04003ED9 RID: 16089
		private static List<string> tmpIdeoMemberNames = new List<string>();
	}
}
