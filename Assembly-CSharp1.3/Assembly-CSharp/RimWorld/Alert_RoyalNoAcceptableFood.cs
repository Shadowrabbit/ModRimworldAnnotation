using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126F RID: 4719
	public class Alert_RoyalNoAcceptableFood : Alert
	{
		// Token: 0x060070F2 RID: 28914 RVA: 0x0025A241 File Offset: 0x00258441
		public Alert_RoyalNoAcceptableFood()
		{
			this.defaultLabel = "RoyalNoAcceptableFood".Translate();
			this.defaultExplanation = "RoyalNoAcceptableFoodDesc".Translate();
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x060070F3 RID: 28915 RVA: 0x0025A280 File Offset: 0x00258480
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (pawn.Spawned && (pawn.story == null || !pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
						{
							Pawn_RoyaltyTracker royalty = pawn.royalty;
							RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
							Thing thing;
							ThingDef thingDef;
							if (royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined && !FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, false, out thing, out thingDef, true, true, false, false, false, false, false, true, false, FoodPreferability.DesperateOnly))
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060070F4 RID: 28916 RVA: 0x0025A38C File Offset: 0x0025858C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060070F5 RID: 28917 RVA: 0x0025A39C File Offset: 0x0025859C
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.Targets.Select(delegate(Pawn t)
			{
				RoyalTitle mostSeniorTitle = t.royalty.MostSeniorTitle;
				string[] array = new string[5];
				array[0] = t.LabelShort;
				array[1] = " (";
				array[2] = mostSeniorTitle.def.GetLabelFor(t.gender);
				array[3] = "):\n";
				array[4] = (from m in mostSeniorTitle.def.SatisfyingMeals(false)
				select m.LabelCap).ToLineList("- ", false);
				string text = string.Concat(array);
				if (ModsConfig.IdeologyActive && t.Ideo != null && t.Ideo.VeneratedAnimals.Any<ThingDef>())
				{
					text = text + "\n\n" + "AlertRoyalTitleNoVeneratedAnimalMeat".Translate(t.Named("PAWN"), t.Ideo.Named("IDEO"), (from x in t.Ideo.VeneratedAnimals
					select x.label).ToCommaList(false, false).Named("ANIMALS")).Resolve();
				}
				return text;
			}).ToLineList("\n", false);
		}

		// Token: 0x04003E30 RID: 15920
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
