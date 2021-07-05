using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1F RID: 3359
	public static class MeditationFocusTypeAvailabilityCache
	{
		// Token: 0x06004D0D RID: 19725 RVA: 0x001ACF34 File Offset: 0x001AB134
		public static bool PawnCanUse(Pawn p, MeditationFocusDef type)
		{
			if (!MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached.ContainsKey(p))
			{
				MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p] = new Dictionary<MeditationFocusDef, bool>();
			}
			if (!MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p].ContainsKey(type))
			{
				MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p][type] = MeditationFocusTypeAvailabilityCache.PawnCanUseInt(p, type);
			}
			return MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p][type];
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0003699A File Offset: 0x00034B9A
		public static void ClearFor(Pawn p)
		{
			if (MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached.ContainsKey(p))
			{
				MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p].Clear();
			}
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x001ACF9C File Offset: 0x001AB19C
		private static bool PawnCanUseInt(Pawn p, MeditationFocusDef type)
		{
			if (p.story != null)
			{
				for (int i = 0; i < p.story.traits.allTraits.Count; i++)
				{
					List<MeditationFocusDef> disallowedMeditationFocusTypes = p.story.traits.allTraits[i].CurrentData.disallowedMeditationFocusTypes;
					if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(type))
					{
						return false;
					}
				}
				Backstory adulthood = p.story.adulthood;
				List<string> list = (adulthood != null) ? adulthood.spawnCategories : null;
				Backstory childhood = p.story.childhood;
				List<string> list2 = (childhood != null) ? childhood.spawnCategories : null;
				for (int j = 0; j < type.incompatibleBackstoriesAny.Count; j++)
				{
					BackstoryCategoryAndSlot backstoryCategoryAndSlot = type.incompatibleBackstoriesAny[j];
					List<string> list3 = (backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? list : list2;
					if (list3 != null && list3.Contains(backstoryCategoryAndSlot.categoryName))
					{
						return false;
					}
				}
			}
			if (!type.requiresRoyalTitle)
			{
				if (p.story != null)
				{
					for (int k = 0; k < p.story.traits.allTraits.Count; k++)
					{
						List<MeditationFocusDef> allowedMeditationFocusTypes = p.story.traits.allTraits[k].CurrentData.allowedMeditationFocusTypes;
						if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(type))
						{
							return true;
						}
					}
					Backstory adulthood2 = p.story.adulthood;
					List<string> list4 = (adulthood2 != null) ? adulthood2.spawnCategories : null;
					Backstory childhood2 = p.story.childhood;
					List<string> list5 = (childhood2 != null) ? childhood2.spawnCategories : null;
					for (int l = 0; l < type.requiredBackstoriesAny.Count; l++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = type.requiredBackstoriesAny[l];
						List<string> list6 = (backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? list4 : list5;
						if (list6 != null && list6.Contains(backstoryCategoryAndSlot2.categoryName))
						{
							return true;
						}
					}
				}
				if (type.requiredBackstoriesAny.Count == 0)
				{
					bool flag = false;
					int num = 0;
					while (num < DefDatabase<TraitDef>.AllDefsListForReading.Count && !flag)
					{
						TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[num];
						for (int m = 0; m < traitDef.degreeDatas.Count; m++)
						{
							List<MeditationFocusDef> allowedMeditationFocusTypes2 = traitDef.degreeDatas[m].allowedMeditationFocusTypes;
							if (allowedMeditationFocusTypes2 != null && allowedMeditationFocusTypes2.Contains(type))
							{
								flag = true;
								break;
							}
						}
						num++;
					}
					if (!flag)
					{
						return true;
					}
				}
				return false;
			}
			if (p.royalty != null)
			{
				return p.royalty.AllTitlesInEffectForReading.Any((RoyalTitle t) => t.def.allowDignifiedMeditationFocus);
			}
			return false;
		}

		// Token: 0x040032B7 RID: 12983
		private static Dictionary<Pawn, Dictionary<MeditationFocusDef, bool>> pawnCanUseMeditationTypeCached = new Dictionary<Pawn, Dictionary<MeditationFocusDef, bool>>();
	}
}
