using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB9 RID: 4025
	public class MeditationFocusDef : Def
	{
		// Token: 0x06005801 RID: 22529 RVA: 0x0003D1AF File Offset: 0x0003B3AF
		public bool CanPawnUse(Pawn p)
		{
			return MeditationFocusTypeAvailabilityCache.PawnCanUse(p, this);
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x001CEFA4 File Offset: 0x001CD1A4
		public string EnablingThingsExplanation(Pawn pawn)
		{
			MeditationFocusDef.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.reasons = new List<string>();
			if (this.requiresRoyalTitle && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				RoyalTitle royalTitle = pawn.royalty.AllTitlesInEffectForReading.MaxBy((RoyalTitle t) => t.def.seniority);
				CS$<>8__locals1.reasons.Add("MeditationFocusEnabledByTitle".Translate(royalTitle.def.GetLabelCapFor(pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).Resolve());
			}
			if (pawn.story != null)
			{
				Backstory adulthood = pawn.story.adulthood;
				Backstory childhood = pawn.story.childhood;
				if (!this.requiresRoyalTitle && this.requiredBackstoriesAny.Count == 0)
				{
					for (int i = 0; i < this.incompatibleBackstoriesAny.Count; i++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = this.incompatibleBackstoriesAny[i];
						Backstory backstory = (backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
						if (!backstory.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName))
						{
							MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|4_0(backstoryCategoryAndSlot.slot, backstory, ref CS$<>8__locals1);
						}
					}
					for (int j = 0; j < DefDatabase<TraitDef>.AllDefsListForReading.Count; j++)
					{
						TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[j];
						List<MeditationFocusDef> disallowedMeditationFocusTypes = traitDef.degreeDatas[0].disallowedMeditationFocusTypes;
						if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(this))
						{
							CS$<>8__locals1.reasons.Add("MeditationFocusDisabledByTrait".Translate() + ": " + traitDef.degreeDatas[0].GetLabelCapFor(pawn) + ".");
						}
					}
				}
				for (int k = 0; k < this.requiredBackstoriesAny.Count; k++)
				{
					BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = this.requiredBackstoriesAny[k];
					Backstory backstory2 = (backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
					if (backstory2.spawnCategories.Contains(backstoryCategoryAndSlot2.categoryName))
					{
						MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|4_0(backstoryCategoryAndSlot2.slot, backstory2, ref CS$<>8__locals1);
					}
				}
				for (int l = 0; l < pawn.story.traits.allTraits.Count; l++)
				{
					Trait trait = pawn.story.traits.allTraits[l];
					List<MeditationFocusDef> allowedMeditationFocusTypes = trait.CurrentData.allowedMeditationFocusTypes;
					if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(this))
					{
						CS$<>8__locals1.reasons.Add("MeditationFocusEnabledByTrait".Translate() + ": " + trait.LabelCap + ".");
					}
				}
			}
			return CS$<>8__locals1.reasons.ToLineList("  - ", true);
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x001CF290 File Offset: 0x001CD490
		[CompilerGenerated]
		internal static void <EnablingThingsExplanation>g__AddBackstoryReason|4_0(BackstorySlot slot, Backstory backstory, ref MeditationFocusDef.<>c__DisplayClass4_0 A_2)
		{
			if (slot == BackstorySlot.Adulthood)
			{
				A_2.reasons.Add("MeditationFocusEnabledByAdulthood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
				return;
			}
			A_2.reasons.Add("MeditationFocusEnabledByChildhood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
		}

		// Token: 0x040039FC RID: 14844
		public bool requiresRoyalTitle;

		// Token: 0x040039FD RID: 14845
		public List<BackstoryCategoryAndSlot> requiredBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		// Token: 0x040039FE RID: 14846
		public List<BackstoryCategoryAndSlot> incompatibleBackstoriesAny = new List<BackstoryCategoryAndSlot>();
	}
}
