using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195D RID: 6493
	public class Alert_RoyalNoAcceptableFood : Alert
	{
		// Token: 0x06008FB9 RID: 36793 RVA: 0x0006053C File Offset: 0x0005E73C
		public Alert_RoyalNoAcceptableFood()
		{
			this.defaultLabel = "RoyalNoAcceptableFood".Translate();
			this.defaultExplanation = "RoyalNoAcceptableFoodDesc".Translate();
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x06008FBA RID: 36794 RVA: 0x002961BC File Offset: 0x002943BC
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
							if (royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined && !FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, false, out thing, out thingDef, true, true, false, false, false, false, false, true, FoodPreferability.DesperateOnly))
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x06008FBB RID: 36795 RVA: 0x00060579 File Offset: 0x0005E779
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06008FBC RID: 36796 RVA: 0x002962C8 File Offset: 0x002944C8
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
				return string.Concat(array);
			}).ToLineList("\n", false);
		}

		// Token: 0x04005B7E RID: 23422
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
