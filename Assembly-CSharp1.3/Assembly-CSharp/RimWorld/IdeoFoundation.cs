using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000ECE RID: 3790
	public abstract class IdeoFoundation : IExposable
	{
		// Token: 0x06005965 RID: 22885
		public abstract void Init(IdeoGenerationParms parms);

		// Token: 0x06005966 RID: 22886
		public abstract void DoInfo(ref float curY, float width, IdeoEditMode editMode);

		// Token: 0x06005967 RID: 22887
		public abstract void GenerateTextSymbols();

		// Token: 0x06005968 RID: 22888 RVA: 0x001E7CD0 File Offset: 0x001E5ED0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<IdeoFoundationDef>(ref this.def, "def");
			Scribe_Defs.Look<PlaceDef>(ref this.place, "place");
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x001E7CF4 File Offset: 0x001E5EF4
		public static IdeoIconDef GetRandomIconDef(Ideo ideo)
		{
			IEnumerable<IdeoIconDef> enumerable = null;
			if (Find.World != null)
			{
				enumerable = from x in DefDatabase<IdeoIconDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo) && !Find.IdeoManager.IdeosListForReading.Any((Ideo y) => y.iconDef == x)
				select x;
			}
			if (enumerable.EnumerableNullOrEmpty<IdeoIconDef>())
			{
				enumerable = from x in DefDatabase<IdeoIconDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo)
				select x;
			}
			return enumerable.RandomElement<IdeoIconDef>();
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x001E7D54 File Offset: 0x001E5F54
		public static ColorDef GetRandomColorDef(Ideo ideo)
		{
			IEnumerable<ColorDef> enumerable = null;
			if (Find.World != null)
			{
				enumerable = from x in DefDatabase<ColorDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo) && !Find.IdeoManager.IdeosListForReading.Any((Ideo y) => y.colorDef == x)
				select x;
			}
			if (enumerable.EnumerableNullOrEmpty<ColorDef>())
			{
				enumerable = from x in DefDatabase<ColorDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo)
				select x;
			}
			return enumerable.RandomElement<ColorDef>();
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x001E7DB4 File Offset: 0x001E5FB4
		public virtual void RandomizeCulture(IdeoGenerationParms parms)
		{
			if (parms.forFaction != null && parms.forFaction.allowedCultures != null)
			{
				this.ideo.culture = parms.forFaction.allowedCultures.RandomElement<CultureDef>();
				return;
			}
			this.ideo.culture = DefDatabase<CultureDef>.AllDefsListForReading.RandomElement<CultureDef>();
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x001E7E07 File Offset: 0x001E6007
		public virtual void RandomizePlace()
		{
			this.place = (from p in DefDatabase<PlaceDef>.AllDefsListForReading
			where p.tags.SharesElementWith(this.ideo.culture.allowedPlaceTags)
			select p).RandomElement<PlaceDef>();
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x001E7E2A File Offset: 0x001E602A
		protected virtual void RandomizeMemes(IdeoGenerationParms parms)
		{
			this.ideo.memes.Clear();
			this.ideo.memes.AddRange(IdeoUtility.GenerateRandomMemes(parms));
			this.ideo.SortMemesInDisplayOrder();
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x001E7E60 File Offset: 0x001E6060
		public virtual void RandomizeStyles()
		{
			if (this.ideo == null)
			{
				return;
			}
			List<ThingStyleCategoryWithPriority> list = new List<ThingStyleCategoryWithPriority>();
			if (!this.ideo.culture.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				list.AddRange(this.ideo.culture.thingStyleCategories);
			}
			List<MemeDef> memes = this.ideo.memes;
			for (int i = 0; i < memes.Count; i++)
			{
				if (!memes[i].thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					list.AddRange(memes[i].thingStyleCategories);
				}
			}
			list.Sort(delegate(ThingStyleCategoryWithPriority first, ThingStyleCategoryWithPriority second)
			{
				if (first.priority != second.priority)
				{
					return -first.priority.CompareTo(second.priority);
				}
				if (Rand.Value >= 0.5f)
				{
					return 1;
				}
				return -1;
			});
			this.ideo.thingStyleCategories = new List<ThingStyleCategoryWithPriority>();
			while (this.ideo.thingStyleCategories.Count < 3 && list.Any<ThingStyleCategoryWithPriority>())
			{
				ThingStyleCategoryWithPriority thingStyleCategoryWithPriority = list.First<ThingStyleCategoryWithPriority>();
				list.Remove(thingStyleCategoryWithPriority);
				if (this.<RandomizeStyles>g__CanUseStyleCategory|21_1(thingStyleCategoryWithPriority.category))
				{
					this.ideo.thingStyleCategories.Add(thingStyleCategoryWithPriority);
				}
			}
			this.ideo.style.ResetStylesForThingDef();
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x001E7F7C File Offset: 0x001E617C
		public bool CanAddForFaction(PreceptDef precept, FactionDef forFaction, List<PreceptDef> disallowedPrecepts, bool checkDuplicates, bool ignoreMemeRequirements = false, bool ignoreConflictingMemes = false)
		{
			if (!this.CanAdd(precept, checkDuplicates).Accepted)
			{
				return false;
			}
			if (disallowedPrecepts != null && disallowedPrecepts.Contains(precept))
			{
				return false;
			}
			if (forFaction != null)
			{
				if (!forFaction.isPlayer && !precept.allowedForNPCFactions)
				{
					return false;
				}
				if (forFaction.disallowedPrecepts != null && forFaction.disallowedPrecepts.Contains(precept))
				{
					return false;
				}
				if (forFaction.classicIdeo)
				{
					if (precept.classic)
					{
						return true;
					}
					if (precept.impact == PreceptImpact.High)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005970 RID: 22896 RVA: 0x001E7FF8 File Offset: 0x001E61F8
		public virtual void RandomizePrecepts(bool init, IdeoGenerationParms parms)
		{
			IdeoFoundation.<>c__DisplayClass27_0 CS$<>8__locals1 = new IdeoFoundation.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.parms = parms;
			this.ideo.ClearPrecepts();
			IdeoFoundation.tmpWeaponClassPairs.Clear();
			IdeoFoundation.tmpInitializedPrecepts.Clear();
			int num = IdeoFoundation.PreceptCountRange_HighImpact.RandomInRange;
			int num2 = IdeoFoundation.PreceptCountRange_MediumImpact.RandomInRange;
			int num3 = IdeoFoundation.PreceptCountRange_LowImpact.RandomInRange;
			bool @bool = Rand.Bool;
			for (int i3 = 0; i3 < this.ideo.memes.Count; i3++)
			{
				if (this.ideo.memes[i3].selectOneOrNone != null && Rand.Value >= this.ideo.memes[i3].selectOneOrNone.noneChance)
				{
					IEnumerable<PreceptThingPair> preceptThingPairs = this.ideo.memes[i3].selectOneOrNone.preceptThingPairs;
					Func<PreceptThingPair, bool> predicate;
					if ((predicate = CS$<>8__locals1.<>9__2) == null)
					{
						predicate = (CS$<>8__locals1.<>9__2 = ((PreceptThingPair x) => CS$<>8__locals1.<>4__this.CanAddForFaction(x.precept, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false)));
					}
					PreceptThingPair preceptThingPair;
					if (preceptThingPairs.Where(predicate).TryRandomElementByWeight((PreceptThingPair x) => x.precept.selectionWeight, out preceptThingPair))
					{
						Precept precept = PreceptMaker.MakePrecept(preceptThingPair.precept);
						Precept_Apparel precept_Apparel;
						Precept_ThingDef precept_ThingDef;
						if ((precept_Apparel = (precept as Precept_Apparel)) != null)
						{
							precept_Apparel.apparelDef = preceptThingPair.thing;
						}
						else if ((precept_ThingDef = (precept as Precept_ThingDef)) != null)
						{
							precept_ThingDef.ThingDef = preceptThingPair.thing;
						}
						this.ideo.AddPrecept(precept, true, null, null);
					}
				}
				if (this.ideo.memes[i3].requiredRituals != null)
				{
					for (int j = 0; j < this.ideo.memes[i3].requiredRituals.Count; j++)
					{
						RequiredRitualAndBuilding requiredRitualAndBuilding = this.ideo.memes[i3].requiredRituals[j];
						Precept_Ritual precept_Ritual = (Precept_Ritual)PreceptMaker.MakePrecept(requiredRitualAndBuilding.precept);
						this.ideo.AddPrecept(precept_Ritual, true, CS$<>8__locals1.parms.forFaction, requiredRitualAndBuilding.pattern);
						precept_Ritual.RegenerateName();
						IdeoFoundation.tmpInitializedPrecepts.Add(precept_Ritual);
						if (requiredRitualAndBuilding.building != null)
						{
							Precept_Building precept_Building = (Precept_Building)PreceptMaker.MakePrecept(PreceptDefOf.IdeoBuilding);
							this.ideo.AddPrecept(precept_Building, true, CS$<>8__locals1.parms.forFaction, null);
							precept_Building.ThingDef = requiredRitualAndBuilding.building;
							IdeoFoundation.tmpInitializedPrecepts.Add(precept_Building);
						}
					}
				}
				if (this.ideo.memes[i3].requireOne != null)
				{
					for (int k = 0; k < this.ideo.memes[i3].requireOne.Count; k++)
					{
						IEnumerable<PreceptDef> source = this.ideo.memes[i3].requireOne[k];
						Func<PreceptDef, bool> predicate2;
						if ((predicate2 = CS$<>8__locals1.<>9__4) == null)
						{
							predicate2 = (CS$<>8__locals1.<>9__4 = ((PreceptDef x) => CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false)));
						}
						PreceptDef preceptDef;
						if (source.Where(predicate2).TryRandomElementByWeight((PreceptDef x) => x.selectionWeight, out preceptDef))
						{
							this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef), true, null, null);
							if (preceptDef.impact == PreceptImpact.High)
							{
								num--;
							}
							else if (preceptDef.impact == PreceptImpact.Medium)
							{
								num2--;
							}
							else
							{
								num3--;
							}
						}
					}
				}
				if (@bool && this.ideo.memes[i3].preferredWeaponClasses != null)
				{
					IdeoFoundation.tmpWeaponClassPairs.Add(this.ideo.memes[i3].preferredWeaponClasses);
				}
			}
			CS$<>8__locals1.<RandomizePrecepts>g__AddPreceptsOfImpact|0(PreceptImpact.High, num);
			CS$<>8__locals1.<RandomizePrecepts>g__AddPreceptsOfImpact|0(PreceptImpact.Medium, num2);
			CS$<>8__locals1.<RandomizePrecepts>g__AddPreceptsOfImpact|0(PreceptImpact.Low, num3);
			List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
			CS$<>8__locals1.allIssueDefs = DefDatabase<IssueDef>.AllDefsListForReading;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.allIssueDefs.Count; i = i2 + 1)
			{
				PreceptDef preceptDef2;
				PreceptDef preceptDef3;
				if (allDefsListForReading.Where(delegate(PreceptDef x)
				{
					if (x.issue == CS$<>8__locals1.allIssueDefs[i])
					{
						List<MemeDef> associatedMemes = x.associatedMemes;
						Predicate<MemeDef> predicate3;
						if ((predicate3 = CS$<>8__locals1.<>9__10) == null)
						{
							predicate3 = (CS$<>8__locals1.<>9__10 = ((MemeDef y) => CS$<>8__locals1.<>4__this.ideo.memes.Contains(y)));
						}
						if (associatedMemes.Any(predicate3))
						{
							return CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false);
						}
					}
					return false;
				}).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out preceptDef2))
				{
					this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef2), false, null, null);
				}
				else if ((from x in allDefsListForReading
				where x.issue == CS$<>8__locals1.allIssueDefs[i] && CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false)
				select x).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out preceptDef3))
				{
					this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef3), false, null, null);
				}
				i2 = i;
			}
			for (int l = 0; l < allDefsListForReading.Count; l++)
			{
				PreceptDef preceptDef4 = allDefsListForReading[l];
				if (!preceptDef4.countsTowardsPreceptLimit && preceptDef4.canGenerateAsSpecialPrecept && this.CanAddForFaction(preceptDef4, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false))
				{
					if (preceptDef4.preceptInstanceCountCurve != null)
					{
						int num4 = Mathf.CeilToInt(preceptDef4.preceptInstanceCountCurve.Evaluate(Rand.Value));
						for (int m = 0; m < num4; m++)
						{
							this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef4), false, null, null);
						}
					}
					else
					{
						this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef4), false, null, null);
					}
				}
			}
			IEnumerable<PreceptDef> enumerable = from x in DefDatabase<PreceptDef>.AllDefs
			where x.preceptClass == typeof(Precept_RoleMulti) && CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false)
			select x;
			if (enumerable.Any<PreceptDef>())
			{
				for (int n = 0; n < 2; n++)
				{
					IEnumerable<PreceptDef> source2 = enumerable;
					Func<PreceptDef, float> weightSelector;
					if ((weightSelector = CS$<>8__locals1.<>9__13) == null)
					{
						weightSelector = (CS$<>8__locals1.<>9__13 = delegate(PreceptDef x)
						{
							if (!CS$<>8__locals1.<>4__this.ideo.HasPrecept(x))
							{
								return 1f;
							}
							return 0f;
						});
					}
					PreceptDef preceptDef5;
					if (source2.TryRandomElementByWeight(weightSelector, out preceptDef5))
					{
						this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef5), false, null, null);
					}
				}
			}
			int num5 = IdeoFoundation.InitialVeneratedAnimalsCountRange.RandomInRange;
			for (int num6 = 0; num6 < this.ideo.memes.Count; num6++)
			{
				if (this.ideo.memes[num6].veneratedAnimalsCountOverride >= 0)
				{
					num5 = this.ideo.memes[num6].veneratedAnimalsCountOverride;
					break;
				}
				num5 += this.ideo.memes[num6].veneratedAnimalsCountOffset;
			}
			for (int num7 = 0; num7 < Mathf.Min(num5, PreceptDefOf.AnimalVenerated.maxCount); num7++)
			{
				Precept_Animal precept2 = (Precept_Animal)PreceptMaker.MakePrecept(PreceptDefOf.AnimalVenerated);
				this.ideo.AddPrecept(precept2, false, null, null);
			}
			if (@bool)
			{
				if (this.ideo.culture.preferredWeaponClasses != null)
				{
					IdeoFoundation.tmpWeaponClassPairs.Add(this.ideo.culture.preferredWeaponClasses);
				}
				Precept_Weapon precept_Weapon = (Precept_Weapon)PreceptMaker.MakePrecept(PreceptDefOf.NobleDespisedWeapons);
				if (!IdeoFoundation.tmpWeaponClassPairs.NullOrEmpty<IdeoWeaponClassPair>())
				{
					IdeoWeaponClassPair ideoWeaponClassPair = IdeoFoundation.tmpWeaponClassPairs.RandomElement<IdeoWeaponClassPair>();
					precept_Weapon.noble = ideoWeaponClassPair.noble;
					precept_Weapon.despised = ideoWeaponClassPair.despised;
				}
				else
				{
					WeaponClassPairDef weaponClassPairDef = DefDatabase<WeaponClassPairDef>.AllDefs.RandomElement<WeaponClassPairDef>();
					bool bool2 = Rand.Bool;
					precept_Weapon.noble = (bool2 ? weaponClassPairDef.second : weaponClassPairDef.first);
					precept_Weapon.despised = (bool2 ? weaponClassPairDef.first : weaponClassPairDef.second);
				}
				this.ideo.AddPrecept(precept_Weapon, false, null, null);
			}
			if (this.ideo.IdeoPrefersNudity())
			{
				List<Precept> preceptsListForReading = this.ideo.PreceptsListForReading;
				for (int num8 = preceptsListForReading.Count - 1; num8 >= 0; num8--)
				{
					if (preceptsListForReading[num8] is Precept_Apparel)
					{
						this.ideo.RemovePrecept(preceptsListForReading[num8], false);
					}
				}
			}
			if (init)
			{
				this.InitPrecepts(CS$<>8__locals1.parms, IdeoFoundation.tmpInitializedPrecepts);
				this.ideo.RecachePrecepts();
			}
			IdeoFoundation.tmpInitializedPrecepts.Clear();
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x001E8814 File Offset: 0x001E6A14
		public void InitPrecepts(IdeoGenerationParms parms, List<Precept> initializedPrecepts = null)
		{
			IdeoFoundation.<>c__DisplayClass28_0 CS$<>8__locals1 = new IdeoFoundation.<>c__DisplayClass28_0();
			CS$<>8__locals1.parms = parms;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.initializedPrecepts = initializedPrecepts;
			Dictionary<MemeDef, int> dictionary = new Dictionary<MemeDef, int>();
			CS$<>8__locals1.extraPrecepts = new List<Precept>();
			CS$<>8__locals1.filledPrecepts = new List<Precept>();
			CS$<>8__locals1.initedPrecepts = new List<Precept>();
			foreach (Precept_Ritual precept_Ritual in this.ideo.PreceptsListForReading.OfType<Precept_Ritual>())
			{
				IdeoFoundation.<>c__DisplayClass28_1 CS$<>8__locals2 = new IdeoFoundation.<>c__DisplayClass28_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				RitualPatternDef ritualPatternDef = null;
				IdeoFoundation.<>c__DisplayClass28_1 CS$<>8__locals3 = CS$<>8__locals2;
				string groupTag;
				if ((groupTag = precept_Ritual.patternGroupTag) == null)
				{
					RitualPatternDef ritualPatternBase = precept_Ritual.def.ritualPatternBase;
					groupTag = ((ritualPatternBase != null) ? ritualPatternBase.patternGroupTag : null);
				}
				CS$<>8__locals3.groupTag = groupTag;
				if (!CS$<>8__locals2.groupTag.NullOrEmpty() && (CS$<>8__locals2.CS$<>8__locals1.initializedPrecepts == null || !CS$<>8__locals2.CS$<>8__locals1.initializedPrecepts.Contains(precept_Ritual)))
				{
					ritualPatternDef = (from d in DefDatabase<RitualPatternDef>.AllDefs
					where d.patternGroupTag == CS$<>8__locals2.groupTag && d.CanFactionUse(CS$<>8__locals2.CS$<>8__locals1.parms.forFaction) && !CS$<>8__locals2.CS$<>8__locals1.<>4__this.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual p) => p.behavior != null && p.sourcePattern == d)
					select d).RandomElementWithFallback(null);
				}
				if (ritualPatternDef != null)
				{
					ritualPatternDef.Fill(precept_Ritual);
					CS$<>8__locals2.CS$<>8__locals1.filledPrecepts.Add(precept_Ritual);
				}
			}
			CS$<>8__locals1.usedMemePatternDefs = new HashSet<RitualPatternDef>();
			foreach (MemeDef memeDef in IdeoFoundation.tmpMemes)
			{
				if (IdeoFoundation.tmpMemesNumRitualsToMake.ContainsKey(memeDef) && (!dictionary.ContainsKey(memeDef) || dictionary[memeDef] < IdeoFoundation.tmpMemesNumRitualsToMake[memeDef]) && !memeDef.replacementPatterns.NullOrEmpty<RitualPatternDef>())
				{
					foreach (Precept precept in from p in this.ideo.PreceptsListForReading
					orderby p is Precept_Ritual descending
					select p)
					{
						RitualPatternDef ritualPatternBase2 = precept.def.ritualPatternBase;
						if (ritualPatternBase2 != null && !ritualPatternBase2.tags.NullOrEmpty<string>() && memeDef.replaceRitualsWithTags.Any(new Predicate<string>(ritualPatternBase2.tags.Contains)))
						{
							int num = 0;
							if (dictionary.ContainsKey(memeDef))
							{
								num = dictionary[memeDef];
							}
							IEnumerable<RitualPatternDef> replacementPatterns = memeDef.replacementPatterns;
							Func<RitualPatternDef, bool> predicate;
							if ((predicate = CS$<>8__locals1.<>9__4) == null)
							{
								predicate = (CS$<>8__locals1.<>9__4 = ((RitualPatternDef p) => p.CanFactionUse(CS$<>8__locals1.parms.forFaction) && !CS$<>8__locals1.usedMemePatternDefs.Contains(p)));
							}
							RitualPatternDef ritualPatternDef2 = replacementPatterns.Where(predicate).RandomElementWithFallback(null);
							if (ritualPatternDef2 != null)
							{
								ritualPatternDef2.Fill((Precept_Ritual)precept);
								dictionary.SetOrAdd(memeDef, num + 1);
								CS$<>8__locals1.filledPrecepts.Add(precept);
								CS$<>8__locals1.usedMemePatternDefs.Add(ritualPatternDef2);
							}
						}
					}
					if (!dictionary.ContainsKey(memeDef) || dictionary[memeDef] < IdeoFoundation.tmpMemesNumRitualsToMake[memeDef])
					{
						foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
						{
							RitualPatternDef pattern = preceptDef.ritualPatternBase;
							if (pattern != null && !pattern.tags.NullOrEmpty<string>() && memeDef.replaceRitualsWithTags.Any(new Predicate<string>(pattern.tags.Contains)) && !CS$<>8__locals1.usedMemePatternDefs.Contains(pattern) && !this.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual r) => r.behavior != null && r.behavior.def == pattern.ritualBehavior))
							{
								Precept precept2 = PreceptMaker.MakePrecept(preceptDef);
								this.ideo.AddPrecept(precept2, false, null, null);
								RitualPatternDef pattern2 = pattern;
								if (pattern2 != null)
								{
									pattern2.Fill((Precept_Ritual)precept2);
								}
								CS$<>8__locals1.filledPrecepts.Add(precept2);
								CS$<>8__locals1.usedMemePatternDefs.Add(pattern);
							}
						}
					}
				}
			}
			using (List<PreceptDef>.Enumerator enumerator4 = DefDatabase<PreceptDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					PreceptDef p = enumerator4.Current;
					if (p.ritualPatternBase != null && !p.requiredMemes.NullOrEmpty<MemeDef>())
					{
						if (p.requiredMemes.Any((MemeDef m) => IdeoFoundation.tmpMemes.Contains(m)) && !this.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual r) => r.sourcePattern == p.ritualPatternBase))
						{
							Precept_Ritual precept_Ritual2 = (Precept_Ritual)PreceptMaker.MakePrecept(p);
							this.ideo.AddPrecept(precept_Ritual2, false, null, null);
							p.ritualPatternBase.Fill(precept_Ritual2);
							CS$<>8__locals1.filledPrecepts.Add(precept_Ritual2);
						}
					}
				}
			}
			CS$<>8__locals1.<InitPrecepts>g__AddAndInitPrecepts|0();
			foreach (Precept_Ritual precept_Ritual3 in this.ideo.PreceptsListForReading.OfType<Precept_Ritual>())
			{
				if (precept_Ritual3.behavior != null && !precept_Ritual3.behavior.def.preceptRequirements.NullOrEmpty<PreceptRequirement>())
				{
					foreach (PreceptRequirement preceptRequirement in precept_Ritual3.behavior.def.preceptRequirements)
					{
						if (!preceptRequirement.Met(this.ideo.PreceptsListForReading) && !preceptRequirement.Met(CS$<>8__locals1.extraPrecepts))
						{
							CS$<>8__locals1.extraPrecepts.Add(preceptRequirement.MakePrecept(this.ideo));
						}
					}
				}
			}
			CS$<>8__locals1.<InitPrecepts>g__AddAndInitPrecepts|0();
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x001E8EC8 File Offset: 0x001E70C8
		public virtual void RandomizeIcon()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ideo.SetIcon(IdeoFoundation.GetRandomIconDef(this.ideo), IdeoFoundation.GetRandomColorDef(this.ideo));
			});
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x001E8EDC File Offset: 0x001E70DC
		public virtual void GenerateLeaderTitle()
		{
			if (this.ideo.culture.leaderTitleMaker == null)
			{
				this.ideo.leaderTitleMale = null;
				this.ideo.leaderTitleFemale = null;
				return;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.ideo.culture.leaderTitleMaker);
			for (int i = 0; i < this.ideo.memes.Count; i++)
			{
				if (this.ideo.memes[i].generalRules != null)
				{
					request.IncludesBare.Add(this.ideo.memes[i].generalRules);
				}
			}
			this.ideo.leaderTitleMale = NameGenerator.GenerateName(request, null, false, "r_leaderTitle", null);
			this.ideo.leaderTitleFemale = this.ideo.leaderTitleMale;
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x001E8FBC File Offset: 0x001E71BC
		public AcceptanceReport CanAdd(PreceptDef precept, bool checkDuplicates = true)
		{
			List<Precept> preceptsListForReading = this.ideo.PreceptsListForReading;
			if (precept.takeNameFrom != null)
			{
				bool flag = false;
				using (List<Precept>.Enumerator enumerator = this.ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def == precept.takeNameFrom)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (precept.preceptClass == typeof(Precept_RoleMulti))
			{
				if (this.ideo.PreceptsListForReading.Count((Precept p) => p is Precept_RoleMulti && p.def.visible) >= 2)
				{
					return "MaxMultiRolesCount".Translate(2);
				}
			}
			if (!precept.requiredMemes.NullOrEmpty<MemeDef>())
			{
				bool flag2 = false;
				foreach (MemeDef item in precept.requiredMemes)
				{
					if (this.ideo.memes.Contains(item))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (precept.requiredMemes.Count == 1)
					{
						return new AcceptanceReport("RequiresMeme".Translate() + ": " + precept.requiredMemes[0].LabelCap);
					}
					return new AcceptanceReport("RequiresOneOfMemes".Translate() + ": " + precept.RequiredMemeLabels.ToCommaList(false, false).CapitalizeFirst());
				}
			}
			int i = 0;
			while (i < precept.conflictingMemes.Count)
			{
				if (this.ideo.memes.Contains(precept.conflictingMemes[i]))
				{
					if (precept.conflictingMemes.Count == 1)
					{
						return new AcceptanceReport("ConflictsWithMeme".Translate() + ": " + precept.conflictingMemes[0].LabelCap);
					}
					return new AcceptanceReport("ConflictsWithMemes".Translate() + ": " + (from m in precept.conflictingMemes
					select m.label).ToCommaList(false, false).CapitalizeFirst());
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < preceptsListForReading.Count; j++)
			{
				if (checkDuplicates)
				{
					if (precept == preceptsListForReading[j].def)
					{
						if (!precept.allowDuplicates)
						{
							return false;
						}
					}
					else if (!precept.issue.allowMultiplePrecepts && precept.issue == preceptsListForReading[j].def.issue)
					{
						return false;
					}
				}
				else if (precept.issue == preceptsListForReading[j].def.issue && this.ideo.PreceptIsRequired(preceptsListForReading[j].def))
				{
					return this.ideo.PreceptIsRequired(precept);
				}
				for (int k = 0; k < precept.exclusionTags.Count; k++)
				{
					if (preceptsListForReading[j].def.exclusionTags.Contains(precept.exclusionTags[k]) && (preceptsListForReading[j].def.issue != precept.issue || checkDuplicates))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x001E9380 File Offset: 0x001E7580
		public void AddPlaceRules(ref GrammarRequest request)
		{
			PlaceDef placeDef = this.place;
			if (((placeDef != null) ? placeDef.placeRules : null) != null)
			{
				request.IncludesBare.Add(this.place.placeRules);
			}
		}

		// Token: 0x06005979 RID: 22905 RVA: 0x001E9448 File Offset: 0x001E7648
		[CompilerGenerated]
		private bool <RandomizeStyles>g__CanUseStyleCategory|21_1(StyleCategoryDef cat)
		{
			if (this.ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority x) => x.category == cat))
			{
				return false;
			}
			Func<ThingDefStyle, bool> <>9__3;
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.ideo.thingStyleCategories)
			{
				IEnumerable<ThingDefStyle> thingDefStyles = thingStyleCategoryWithPriority.category.thingDefStyles;
				Func<ThingDefStyle, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((ThingDefStyle x) => cat.thingDefStyles.Contains(x)));
				}
				if (thingDefStyles.All(predicate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400346C RID: 13420
		public Ideo ideo;

		// Token: 0x0400346D RID: 13421
		public IdeoFoundationDef def;

		// Token: 0x0400346E RID: 13422
		public PlaceDef place;

		// Token: 0x0400346F RID: 13423
		public static readonly IntRange MemeCountRangeAbsolute = new IntRange(1, 4);

		// Token: 0x04003470 RID: 13424
		public static readonly IntRange MemeCountRangeNPCInitial = new IntRange(1, 3);

		// Token: 0x04003471 RID: 13425
		private static IntRange PreceptCountRange_HighImpact = new IntRange(0, 2);

		// Token: 0x04003472 RID: 13426
		private static IntRange PreceptCountRange_MediumImpact = new IntRange(5, 5);

		// Token: 0x04003473 RID: 13427
		private static IntRange PreceptCountRange_LowImpact = new IntRange(5, 5);

		// Token: 0x04003474 RID: 13428
		private static IntRange InitialVeneratedAnimalsCountRange = new IntRange(0, 1);

		// Token: 0x04003475 RID: 13429
		public const int MaxStyleCategories = 3;

		// Token: 0x04003476 RID: 13430
		public const int MaxRituals = 6;

		// Token: 0x04003477 RID: 13431
		public const int MaxMultiRoles = 2;

		// Token: 0x04003478 RID: 13432
		private static List<MemeDef> tmpMemes = new List<MemeDef>();

		// Token: 0x04003479 RID: 13433
		private static Dictionary<MemeDef, int> tmpMemesNumRitualsToMake = new Dictionary<MemeDef, int>();

		// Token: 0x0400347A RID: 13434
		private static List<IdeoWeaponClassPair> tmpWeaponClassPairs = new List<IdeoWeaponClassPair>();

		// Token: 0x0400347B RID: 13435
		private static List<Precept> tmpInitializedPrecepts = new List<Precept>();
	}
}
