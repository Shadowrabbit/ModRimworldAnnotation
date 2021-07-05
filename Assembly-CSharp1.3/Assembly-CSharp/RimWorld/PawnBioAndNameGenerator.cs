using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBD RID: 3517
	public static class PawnBioAndNameGenerator
	{
		// Token: 0x06005159 RID: 20825 RVA: 0x001B5984 File Offset: 0x001B3B84
		public static void GiveAppropriateBioAndNameTo(Pawn pawn, string requiredLastName, FactionDef factionType, bool forceNoBackstory = false)
		{
			List<BackstoryCategoryFilter> backstoryCategoryFiltersFor = PawnBioAndNameGenerator.GetBackstoryCategoryFiltersFor(pawn, factionType);
			if (!forceNoBackstory && (Rand.Value < 0.25f || pawn.kindDef.factionLeader) && PawnBioAndNameGenerator.TryGiveSolidBioTo(pawn, requiredLastName, backstoryCategoryFiltersFor))
			{
				return;
			}
			PawnBioAndNameGenerator.GiveShuffledBioTo(pawn, factionType, requiredLastName, backstoryCategoryFiltersFor, forceNoBackstory);
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001B59CC File Offset: 0x001B3BCC
		private static void GiveShuffledBioTo(Pawn pawn, FactionDef factionType, string requiredLastName, List<BackstoryCategoryFilter> backstoryCategories, bool forceNoBackstory = false)
		{
			if (!forceNoBackstory)
			{
				bool flag = pawn.ageTracker.AgeBiologicalYearsFloat >= 20f;
				PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Childhood, ref pawn.story.childhood, pawn.story.adulthood, backstoryCategories, factionType, flag ? new BackstorySlot?(BackstorySlot.Adulthood) : null);
				if (flag)
				{
					PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Adulthood, ref pawn.story.adulthood, pawn.story.childhood, backstoryCategories, factionType, null);
				}
			}
			pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, requiredLastName);
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x001B5A60 File Offset: 0x001B3C60
		private static void FillBackstorySlotShuffled(Pawn pawn, BackstorySlot slot, ref Backstory backstory, Backstory backstoryOtherSlot, List<BackstoryCategoryFilter> backstoryCategories, FactionDef factionType, BackstorySlot? mustBeCompatibleTo = null)
		{
			BackstoryCategoryFilter backstoryCategoryFilter = backstoryCategories.RandomElementByWeight((BackstoryCategoryFilter c) => c.commonality);
			if (backstoryCategoryFilter == null)
			{
				backstoryCategoryFilter = PawnBioAndNameGenerator.FallbackCategoryGroup;
			}
			if (!(from bs in BackstoryDatabase.ShuffleableBackstoryList(slot, backstoryCategoryFilter, mustBeCompatibleTo).TakeRandom(20)
			where slot != BackstorySlot.Adulthood || !bs.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.childhood.workDisables)
			select bs).TryRandomElementByWeight(new Func<Backstory, float>(PawnBioAndNameGenerator.BackstorySelectionWeight), out backstory))
			{
				Log.Error(string.Concat(new object[]
				{
					"No shuffled ",
					slot,
					" found for ",
					pawn.ToStringSafe<Pawn>(),
					" of ",
					factionType.ToStringSafe<FactionDef>(),
					". Choosing random."
				}));
				backstory = (from kvp in BackstoryDatabase.allBackstories
				where kvp.Value.slot == slot
				select kvp).RandomElement<KeyValuePair<string, Backstory>>().Value;
			}
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001B5B68 File Offset: 0x001B3D68
		private static bool TryGiveSolidBioTo(Pawn pawn, string requiredLastName, List<BackstoryCategoryFilter> backstoryCategories)
		{
			PawnBio pawnBio;
			if (!PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(backstoryCategories, pawn.kindDef, pawn.gender, requiredLastName, out pawnBio))
			{
				return false;
			}
			if (pawnBio.name.First == "Tynan" && pawnBio.name.Last == "Sylvester" && Rand.Value < 0.5f && !PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(backstoryCategories, pawn.kindDef, pawn.gender, requiredLastName, out pawnBio))
			{
				return false;
			}
			pawn.Name = pawnBio.name;
			pawn.story.childhood = pawnBio.childhood;
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				pawn.story.adulthood = pawnBio.adulthood;
			}
			return true;
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001B5C24 File Offset: 0x001B3E24
		private static bool IsBioUseable(PawnBio bio, BackstoryCategoryFilter categoryFilter, PawnKindDef kind, Gender gender, string requiredLastName)
		{
			if (bio.gender != GenderPossibility.Either)
			{
				if (gender == Gender.Male && bio.gender != GenderPossibility.Male)
				{
					return false;
				}
				if (gender == Gender.Female && bio.gender != GenderPossibility.Female)
				{
					return false;
				}
			}
			if (!requiredLastName.NullOrEmpty() && bio.name.Last != requiredLastName)
			{
				return false;
			}
			if (kind.factionLeader && !bio.pirateKing)
			{
				return false;
			}
			if (!categoryFilter.Matches(bio))
			{
				return false;
			}
			if (bio.name.UsedThisGame)
			{
				return false;
			}
			if (kind.requiredWorkTags != WorkTags.None)
			{
				if (bio.childhood != null && (bio.childhood.workDisables & kind.requiredWorkTags) != WorkTags.None)
				{
					return false;
				}
				if (bio.adulthood != null && (bio.adulthood.workDisables & kind.requiredWorkTags) != WorkTags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x001B5CE8 File Offset: 0x001B3EE8
		private static bool TryGetRandomUnusedSolidBioFor(List<BackstoryCategoryFilter> backstoryCategories, PawnKindDef kind, Gender gender, string requiredLastName, out PawnBio result)
		{
			BackstoryCategoryFilter categoryFilter = backstoryCategories.RandomElementByWeight((BackstoryCategoryFilter c) => c.commonality);
			if (categoryFilter == null)
			{
				categoryFilter = PawnBioAndNameGenerator.FallbackCategoryGroup;
			}
			if (Rand.Value < 0.5f)
			{
				PawnBioAndNameGenerator.tmpNames.Clear();
				PawnBioAndNameGenerator.tmpNames.AddRange(Prefs.PreferredNames);
				PawnBioAndNameGenerator.tmpNames.Shuffle<string>();
				foreach (string a in PawnBioAndNameGenerator.tmpNames)
				{
					foreach (PawnBio pawnBio in SolidBioDatabase.allBios)
					{
						if (a == pawnBio.name.ToString() && PawnBioAndNameGenerator.IsBioUseable(pawnBio, categoryFilter, kind, gender, requiredLastName))
						{
							result = pawnBio;
							return true;
						}
					}
				}
			}
			return (from bio in SolidBioDatabase.allBios.TakeRandom(20)
			where PawnBioAndNameGenerator.IsBioUseable(bio, categoryFilter, kind, gender, requiredLastName)
			select bio).TryRandomElementByWeight(new Func<PawnBio, float>(PawnBioAndNameGenerator.BioSelectionWeight), out result);
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001B5E6C File Offset: 0x001B406C
		public static NameTriple TryGetRandomUnusedSolidName(Gender gender, string requiredLastName = null)
		{
			List<NameTriple> listForGender = PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Either);
			List<NameTriple> list = (gender == Gender.Male) ? PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Male) : PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Female);
			float num = ((float)listForGender.Count + 0.1f) / ((float)(listForGender.Count + list.Count) + 0.1f);
			List<NameTriple> list2;
			if (Rand.Value < num)
			{
				list2 = listForGender;
			}
			else
			{
				list2 = list;
			}
			if (list2.Count == 0)
			{
				Log.Error("Empty solid pawn name list for gender: " + gender + ".");
				return null;
			}
			if (Rand.Value < 0.5f)
			{
				PawnBioAndNameGenerator.tmpNames.Clear();
				PawnBioAndNameGenerator.tmpNames.AddRange(Prefs.PreferredNames);
				PawnBioAndNameGenerator.tmpNames.Shuffle<string>();
				foreach (string rawName in PawnBioAndNameGenerator.tmpNames)
				{
					NameTriple nameTriple = NameTriple.FromString(rawName);
					if (list2.Contains(nameTriple) && !nameTriple.UsedThisGame && (requiredLastName == null || !(nameTriple.Last != requiredLastName)))
					{
						return nameTriple;
					}
				}
			}
			list2.Shuffle<NameTriple>();
			return (from name in list2
			where (requiredLastName == null || !(name.Last != requiredLastName)) && !name.UsedThisGame
			select name).FirstOrDefault<NameTriple>();
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001B5FC4 File Offset: 0x001B41C4
		private static List<BackstoryCategoryFilter> GetBackstoryCategoryFiltersFor(Pawn pawn, FactionDef faction)
		{
			if (!pawn.kindDef.backstoryFiltersOverride.NullOrEmpty<BackstoryCategoryFilter>())
			{
				return pawn.kindDef.backstoryFiltersOverride;
			}
			List<BackstoryCategoryFilter> list = new List<BackstoryCategoryFilter>();
			if (pawn.kindDef.backstoryFilters != null)
			{
				list.AddRange(pawn.kindDef.backstoryFilters);
			}
			if (faction != null && !faction.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
			{
				for (int i = 0; i < faction.backstoryFilters.Count; i++)
				{
					BackstoryCategoryFilter item = faction.backstoryFilters[i];
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			if (!list.NullOrEmpty<BackstoryCategoryFilter>())
			{
				return list;
			}
			Log.ErrorOnce(string.Concat(new object[]
			{
				"PawnKind ",
				pawn.kindDef,
				" generating with factionDef ",
				faction,
				": no backstoryCategories in either."
			}), 1871521);
			return new List<BackstoryCategoryFilter>
			{
				PawnBioAndNameGenerator.FallbackCategoryGroup
			};
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001B60AC File Offset: 0x001B42AC
		public static Name GeneratePawnName(Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null)
		{
			if (style == NameStyle.Full)
			{
				ThingDef def = pawn.def;
				RulePackDef nameMaker = pawn.kindDef.GetNameMaker(pawn.gender);
				Pawn_StoryTracker story = pawn.story;
				RulePackDef nameGenerator = pawn.RaceProps.GetNameGenerator(pawn.gender);
				Faction faction = pawn.Faction;
				CultureDef primaryCulture;
				if (faction == null)
				{
					primaryCulture = null;
				}
				else
				{
					FactionIdeosTracker ideos = faction.ideos;
					primaryCulture = ((ideos != null) ? ideos.PrimaryCulture : null);
				}
				return PawnBioAndNameGenerator.GenerateFullPawnName(def, nameMaker, story, nameGenerator, primaryCulture, pawn.gender, pawn.RaceProps.nameCategory, forcedLastName);
			}
			if (style == NameStyle.Numeric)
			{
				try
				{
					foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
					{
						NameSingle nameSingle = pawn2.Name as NameSingle;
						if (nameSingle != null)
						{
							PawnBioAndNameGenerator.usedNamesTmp.Add(nameSingle.Name);
						}
					}
					int num = 1;
					string text;
					for (;;)
					{
						text = string.Format("{0} {1}", pawn.KindLabel, num.ToString());
						if (!PawnBioAndNameGenerator.usedNamesTmp.Contains(text))
						{
							break;
						}
						num++;
					}
					return new NameSingle(text, true);
				}
				finally
				{
					PawnBioAndNameGenerator.usedNamesTmp.Clear();
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x001B61DC File Offset: 0x001B43DC
		public static Name GenerateFullPawnName(ThingDef genFor, RulePackDef pawnKindNameMaker = null, Pawn_StoryTracker story = null, RulePackDef nameGenner = null, CultureDef primaryCulture = null, Gender gender = Gender.None, PawnNameCategory nameCategory = PawnNameCategory.HumanStandard, string forcedLastName = null)
		{
			if (pawnKindNameMaker != null)
			{
				return PawnBioAndNameGenerator.NameResolvedFrom(pawnKindNameMaker, forcedLastName);
			}
			if (story != null)
			{
				Backstory childhood = story.childhood;
				if (((childhood != null) ? childhood.NameMaker : null) != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(story.childhood.NameMaker, forcedLastName);
				}
				Backstory adulthood = story.adulthood;
				if (((adulthood != null) ? adulthood.NameMaker : null) != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(story.adulthood.NameMaker, forcedLastName);
				}
			}
			if (nameGenner != null)
			{
				return new NameSingle(NameGenerator.GenerateName(nameGenner, (string x) => !new NameSingle(x, false).UsedThisGame, false, null, null), false);
			}
			if (primaryCulture != null)
			{
				RulePackDef pawnNameMaker = primaryCulture.GetPawnNameMaker(gender);
				if (pawnNameMaker != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(pawnNameMaker, forcedLastName);
				}
			}
			if (nameCategory != PawnNameCategory.NoName)
			{
				if (Rand.Value < 0.5f)
				{
					NameTriple nameTriple = PawnBioAndNameGenerator.TryGetRandomUnusedSolidName(gender, forcedLastName);
					if (nameTriple != null)
					{
						return nameTriple;
					}
				}
				return PawnBioAndNameGenerator.GeneratePawnName_Shuffled(nameCategory, gender, forcedLastName);
			}
			Log.Error("No name making method for " + genFor);
			NameTriple nameTriple2 = NameTriple.FromString(genFor.label);
			nameTriple2.ResolveMissingPieces(null);
			return nameTriple2;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x001B62E0 File Offset: 0x001B44E0
		private static Name NameResolvedFrom(RulePackDef nameMaker, string forcedLastName)
		{
			NameTriple nameTriple = NameTriple.FromString(NameGenerator.GenerateName(nameMaker, delegate(string x)
			{
				NameTriple nameTriple2 = NameTriple.FromString(x);
				nameTriple2.ResolveMissingPieces(forcedLastName);
				return !nameTriple2.UsedThisGame;
			}, false, null, null));
			nameTriple.CapitalizeNick();
			nameTriple.ResolveMissingPieces(forcedLastName);
			return nameTriple;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001B6328 File Offset: 0x001B4528
		private static NameTriple GeneratePawnName_Shuffled(PawnNameCategory nType, Gender gender, string forcedLastName = null)
		{
			if (nType == PawnNameCategory.NoName)
			{
				Log.Message("Can't create a name of type NoName. Defaulting to HumanStandard.");
				nType = PawnNameCategory.HumanStandard;
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(nType);
			string name = nameBank.GetName(PawnNameSlot.First, gender, true);
			string text;
			if (forcedLastName != null)
			{
				text = forcedLastName;
			}
			else
			{
				text = nameBank.GetName(PawnNameSlot.Last, Gender.None, true);
			}
			int num = 0;
			string nick;
			do
			{
				num++;
				if (Rand.Value < 0.15f)
				{
					Gender gender2 = gender;
					if (Rand.Value < 0.8f)
					{
						gender2 = Gender.None;
					}
					nick = nameBank.GetName(PawnNameSlot.Nick, gender2, true);
				}
				else if (Rand.Value < 0.5f)
				{
					nick = name;
				}
				else
				{
					nick = text;
				}
			}
			while (num < 50 && NameUseChecker.AllPawnsNamesEverUsed.Any(delegate(Name x)
			{
				NameTriple nameTriple = x as NameTriple;
				return nameTriple != null && nameTriple.Nick == nick;
			}));
			return new NameTriple(name, nick, text);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001B63ED File Offset: 0x001B45ED
		private static float BackstorySelectionWeight(Backstory bs)
		{
			return PawnBioAndNameGenerator.SelectionWeightFactorFromWorkTagsDisabled(bs.workDisables);
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001B63FA File Offset: 0x001B45FA
		private static float BioSelectionWeight(PawnBio bio)
		{
			return PawnBioAndNameGenerator.SelectionWeightFactorFromWorkTagsDisabled(bio.adulthood.workDisables | bio.childhood.workDisables);
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x001B6418 File Offset: 0x001B4618
		private static float SelectionWeightFactorFromWorkTagsDisabled(WorkTags wt)
		{
			float num = 1f;
			if ((wt & WorkTags.ManualDumb) != WorkTags.None)
			{
				num *= 0.5f;
			}
			if ((wt & WorkTags.ManualSkilled) != WorkTags.None)
			{
				num *= 1f;
			}
			if ((wt & WorkTags.Violent) != WorkTags.None)
			{
				num *= 0.6f;
			}
			if ((wt & WorkTags.Social) != WorkTags.None)
			{
				num *= 0.7f;
			}
			if ((wt & WorkTags.Intellectual) != WorkTags.None)
			{
				num *= 0.4f;
			}
			if ((wt & WorkTags.Firefighting) != WorkTags.None)
			{
				num *= 0.8f;
			}
			return num;
		}

		// Token: 0x04003036 RID: 12342
		private const float MinAgeForAdulthood = 20f;

		// Token: 0x04003037 RID: 12343
		private const float SolidBioChance = 0.25f;

		// Token: 0x04003038 RID: 12344
		private const float SolidNameChance = 0.5f;

		// Token: 0x04003039 RID: 12345
		private const float TryPreferredNameChance_Bio = 0.5f;

		// Token: 0x0400303A RID: 12346
		private const float TryPreferredNameChance_Name = 0.5f;

		// Token: 0x0400303B RID: 12347
		private const float ShuffledNicknameChance = 0.15f;

		// Token: 0x0400303C RID: 12348
		private const float ShuffledNicknameChanceImperial = 0.05f;

		// Token: 0x0400303D RID: 12349
		private const float ShuffledNicknameChanceUnisex = 0.8f;

		// Token: 0x0400303E RID: 12350
		private static readonly BackstoryCategoryFilter FallbackCategoryGroup = new BackstoryCategoryFilter
		{
			categories = new List<string>
			{
				"Civil"
			},
			commonality = 1f
		};

		// Token: 0x0400303F RID: 12351
		private static List<string> tmpNames = new List<string>();

		// Token: 0x04003040 RID: 12352
		private static HashSet<string> usedNamesTmp = new HashSet<string>();
	}
}
