using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A8F RID: 2703
	public class MeditationFocusDef : Def
	{
		// Token: 0x06004079 RID: 16505 RVA: 0x0015CBAE File Offset: 0x0015ADAE
		public bool CanPawnUse(Pawn p)
		{
			return MeditationFocusTypeAvailabilityCache.PawnCanUse(p, this);
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0015CBB8 File Offset: 0x0015ADB8
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

		// Token: 0x0600407C RID: 16508 RVA: 0x0015CEC4 File Offset: 0x0015B0C4
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

		// Token: 0x04002517 RID: 9495
		public bool requiresRoyalTitle;

		// Token: 0x04002518 RID: 9496
		public List<BackstoryCategoryAndSlot> requiredBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		// Token: 0x04002519 RID: 9497
		public List<BackstoryCategoryAndSlot> incompatibleBackstoriesAny = new List<BackstoryCategoryAndSlot>();
	}
}
