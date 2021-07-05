using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000A98 RID: 2712
	public class MemeDef : Def
	{
		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x0015CF61 File Offset: 0x0015B161
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004085 RID: 16517 RVA: 0x0015CF8C File Offset: 0x0015B18C
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x0015D05C File Offset: 0x0015B25C
		public List<string> UnlockedRoles(Ideo ideo)
		{
			if (this.unlockedRoles == null || ideo != this.unlockedRolesCachedFor)
			{
				this.unlockedRoles = new List<string>();
				using (List<PreceptDef>.Enumerator enumerator = DefDatabase<PreceptDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PreceptDef p = enumerator.Current;
						if (typeof(Precept_Role).IsAssignableFrom(p.preceptClass) && !p.requiredMemes.NullOrEmpty<MemeDef>() && p.requiredMemes.Contains(this))
						{
							if (ideo != null)
							{
								List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
								if (preceptsListForReading != null)
								{
									preceptsListForReading.FirstOrDefault((Precept pc) => pc.def == p);
								}
							}
							this.unlockedRoles.Add(p.LabelCap.Resolve());
						}
					}
				}
				this.unlockedRolesCachedFor = ideo;
			}
			return this.unlockedRoles;
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0015D164 File Offset: 0x0015B364
		public List<string> UnlockedRituals()
		{
			if (this.unlockedRituals == null)
			{
				this.unlockedRituals = new List<string>();
				if (this.consumableBuildings != null)
				{
					using (List<RitualPatternDef>.Enumerator enumerator = DefDatabase<RitualPatternDef>.AllDefsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RitualPatternDef p = enumerator.Current;
							if (!p.ignoreConsumableBuildingRequirement && p.ritualObligationTargetFilter != null && !p.ritualObligationTargetFilter.thingDefs.NullOrEmpty<ThingDef>() && p.ritualObligationTargetFilter.thingDefs.Any((ThingDef td) => this.consumableBuildings.Contains(td)))
							{
								string text;
								if ((text = p.shortDescOverride.CapitalizeFirst()) == null)
								{
									PreceptDef preceptDef = (from pr in DefDatabase<PreceptDef>.AllDefsListForReading
									where pr.ritualPatternBase == p
									select pr).FirstOrDefault<PreceptDef>();
									TaggedString? taggedString = (preceptDef != null) ? new TaggedString?(preceptDef.LabelCap) : null;
									text = ((taggedString != null) ? taggedString.GetValueOrDefault() : null);
								}
								string text2 = text;
								if (text2 != null)
								{
									this.unlockedRituals.Add(text2);
								}
							}
						}
					}
				}
			}
			return this.unlockedRituals;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0015D2B8 File Offset: 0x0015B4B8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category == MemeCategory.Structure && string.IsNullOrWhiteSpace(this.worshipRoomLabel))
			{
				yield return "Structure meme has empty worshipRoomLabel";
			}
			if (this.fixedDeityNameTypes != null && this.category != MemeCategory.Structure)
			{
				yield return "fixedDeityNameTypes can only be used in structure memes.";
			}
			if (!this.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.priority <= 0f)
					{
						yield return "style category " + thingStyleCategoryWithPriority.category.LabelCap + " has <= 0 priority. It must be positive.";
					}
				}
				List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = default(List<ThingStyleCategoryWithPriority>.Enumerator);
			}
			if (this.category == MemeCategory.Structure && this.impact != 0)
			{
				yield return this.defName + " structure meme impact must be 0.";
			}
			if (this.category != MemeCategory.Structure && (this.impact < 1 || this.impact > 3))
			{
				yield return string.Format("{0} normal meme impact must be between 1 and {1}.", this.defName, 3);
			}
			if (this.category == MemeCategory.Structure)
			{
				if (this.descriptionMaker == null)
				{
					yield return "descriptionMaker is required for structure memes.";
				}
				else
				{
					if (this.descriptionMaker.patterns.NullOrEmpty<IdeoDescriptionMaker.PatternEntry>())
					{
						yield return "descriptionMaker must define at least one pattern for structure memes.";
					}
					using (List<IdeoDescriptionMaker.PatternEntry>.Enumerator enumerator3 = this.descriptionMaker.patterns.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.def == null)
							{
								yield return "descriptionMaker pattern has null def.";
							}
						}
					}
					List<IdeoDescriptionMaker.PatternEntry>.Enumerator enumerator3 = default(List<IdeoDescriptionMaker.PatternEntry>.Enumerator);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400252F RID: 9519
		public string iconPath;

		// Token: 0x04002530 RID: 9520
		public int renderOrder;

		// Token: 0x04002531 RID: 9521
		public int impact = -999;

		// Token: 0x04002532 RID: 9522
		public MemeCategory category;

		// Token: 0x04002533 RID: 9523
		public MemeGroupDef groupDef;

		// Token: 0x04002534 RID: 9524
		[NoTranslate]
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04002535 RID: 9525
		public List<List<PreceptDef>> requireOne;

		// Token: 0x04002536 RID: 9526
		public PreceptsWithNoneChance selectOneOrNone;

		// Token: 0x04002537 RID: 9527
		public List<RequiredRitualAndBuilding> requiredRituals;

		// Token: 0x04002538 RID: 9528
		public IdeoWeaponClassPair preferredWeaponClasses;

		// Token: 0x04002539 RID: 9529
		public float randomizationSelectionWeightFactor = 1f;

		// Token: 0x0400253A RID: 9530
		public List<ThingDef> requireAnyRitualSeat;

		// Token: 0x0400253B RID: 9531
		public IntRange deityCount = new IntRange(-1, -1);

		// Token: 0x0400253C RID: 9532
		public bool allowDuringTutorial;

		// Token: 0x0400253D RID: 9533
		public RulePack generalRules;

		// Token: 0x0400253E RID: 9534
		public IdeoDescriptionMaker descriptionMaker;

		// Token: 0x0400253F RID: 9535
		public RulePackDef deityNameMakerOverride;

		// Token: 0x04002540 RID: 9536
		public RulePackDef deityTypeMakerOverride;

		// Token: 0x04002541 RID: 9537
		public bool allowSymbolsFromDeity = true;

		// Token: 0x04002542 RID: 9538
		public bool symbolPackOverride;

		// Token: 0x04002543 RID: 9539
		[MustTranslate]
		public List<IdeoSymbolPack> symbolPacks;

		// Token: 0x04002544 RID: 9540
		public List<DeityNameType> fixedDeityNameTypes;

		// Token: 0x04002545 RID: 9541
		public List<ThingStyleCategoryWithPriority> thingStyleCategories;

		// Token: 0x04002546 RID: 9542
		public IntRange ritualsToMake = IntRange.zero;

		// Token: 0x04002547 RID: 9543
		public List<string> replaceRitualsWithTags = new List<string>();

		// Token: 0x04002548 RID: 9544
		public List<RitualPatternDef> replacementPatterns;

		// Token: 0x04002549 RID: 9545
		public List<ThingDef> consumableBuildings;

		// Token: 0x0400254A RID: 9546
		public int veneratedAnimalsCountOffset;

		// Token: 0x0400254B RID: 9547
		public int veneratedAnimalsCountOverride = -1;

		// Token: 0x0400254C RID: 9548
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x0400254D RID: 9549
		[MustTranslate]
		public string worshipRoomLabel;

		// Token: 0x0400254E RID: 9550
		public List<ResearchProjectDef> startingResearchProjects = new List<ResearchProjectDef>();

		// Token: 0x0400254F RID: 9551
		public List<BuildableDef> addDesignators;

		// Token: 0x04002550 RID: 9552
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x04002551 RID: 9553
		public List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x04002552 RID: 9554
		private Texture2D icon;

		// Token: 0x04002553 RID: 9555
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x04002554 RID: 9556
		private List<string> unlockedRoles;

		// Token: 0x04002555 RID: 9557
		private List<string> unlockedRituals;

		// Token: 0x04002556 RID: 9558
		private Ideo unlockedRolesCachedFor;
	}
}
