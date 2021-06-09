using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200089F RID: 2207
	public class ThingFilter : IExposable
	{
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060036B3 RID: 14003 RVA: 0x0015D150 File Offset: 0x0015B350
		public string Summary
		{
			get
			{
				if (!this.customSummary.NullOrEmpty())
				{
					return this.customSummary;
				}
				if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty<string>() && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return this.thingDefs[0].label;
				}
				if (this.thingDefs.NullOrEmpty<ThingDef>() && this.categories != null && this.categories.Count == 1 && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label;
				}
				if (this.allowedDefs.Count == 1)
				{
					return this.allowedDefs.First<ThingDef>().label;
				}
				return "UsableIngredients".Translate();
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x0015D3B8 File Offset: 0x0015B5B8
		public ThingRequest BestThingRequest
		{
			get
			{
				if (this.allowedDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.allowedDefs.First<ThingDef>());
				}
				bool flag = true;
				bool flag2 = true;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (!thingDef.EverHaulable)
					{
						flag = false;
					}
					if (thingDef.category != ThingCategory.Pawn)
					{
						flag2 = false;
					}
				}
				if (flag)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
				}
				if (flag2)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.Everything);
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060036B5 RID: 14005 RVA: 0x0002A708 File Offset: 0x00028908
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060036B6 RID: 14006 RVA: 0x0002A715 File Offset: 0x00028915
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060036B7 RID: 14007 RVA: 0x0002A71D File Offset: 0x0002891D
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x0002A748 File Offset: 0x00028948
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x0002A755 File Offset: 0x00028955
		// (set) Token: 0x060036BA RID: 14010 RVA: 0x0002A75D File Offset: 0x0002895D
		public FloatRange AllowedHitPointsPercents
		{
			get
			{
				return this.allowedHitPointsPercents;
			}
			set
			{
				if (this.allowedHitPointsPercents == value)
				{
					return;
				}
				this.allowedHitPointsPercents = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060036BB RID: 14011 RVA: 0x0002A788 File Offset: 0x00028988
		// (set) Token: 0x060036BC RID: 14012 RVA: 0x0002A790 File Offset: 0x00028990
		public QualityRange AllowedQualityLevels
		{
			get
			{
				return this.allowedQualities;
			}
			set
			{
				if (this.allowedQualities == value)
				{
					return;
				}
				this.allowedQualities = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x0002A7BB File Offset: 0x000289BB
		// (set) Token: 0x060036BE RID: 14014 RVA: 0x0002A7DF File Offset: 0x000289DF
		public TreeNode_ThingCategory DisplayRootCategory
		{
			get
			{
				if (this.displayRootCategoryInt == null)
				{
					this.RecalculateDisplayRootCategory();
				}
				if (this.displayRootCategoryInt == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.displayRootCategoryInt;
			}
			set
			{
				if (value == this.displayRootCategoryInt)
				{
					return;
				}
				this.displayRootCategoryInt = value;
				this.RecalculateSpecialFilterConfigurability();
			}
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0015D458 File Offset: 0x0015B658
		public ThingFilter()
		{
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x0015D4B0 File Offset: 0x0015B6B0
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x0015D510 File Offset: 0x0015B710
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x0015D578 File Offset: 0x0015B778
		public void ResolveReferences()
		{
			for (int i = 0; i < DefDatabase<SpecialThingFilterDef>.AllDefsListForReading.Count; i++)
			{
				SpecialThingFilterDef specialThingFilterDef = DefDatabase<SpecialThingFilterDef>.AllDefsListForReading[i];
				if (!specialThingFilterDef.allowedByDefault)
				{
					this.SetAllow(specialThingFilterDef, false);
				}
			}
			if (this.thingDefs != null)
			{
				for (int j = 0; j < this.thingDefs.Count; j++)
				{
					if (this.thingDefs[j] != null)
					{
						this.SetAllow(this.thingDefs[j], true);
					}
					else
					{
						Log.Error("ThingFilter could not find thing def named " + this.thingDefs[j], false);
					}
				}
			}
			if (this.categories != null)
			{
				for (int k = 0; k < this.categories.Count; k++)
				{
					ThingCategoryDef named = DefDatabase<ThingCategoryDef>.GetNamed(this.categories[k], true);
					if (named != null)
					{
						this.SetAllow(named, true, null, null);
					}
				}
			}
			if (this.tradeTagsToAllow != null)
			{
				for (int l = 0; l < this.tradeTagsToAllow.Count; l++)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int m = 0; m < allDefsListForReading.Count; m++)
					{
						ThingDef thingDef = allDefsListForReading[m];
						if (thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tradeTagsToAllow[l]))
						{
							this.SetAllow(thingDef, true);
						}
					}
				}
			}
			if (this.tradeTagsToDisallow != null)
			{
				for (int n = 0; n < this.tradeTagsToDisallow.Count; n++)
				{
					List<ThingDef> allDefsListForReading2 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num = 0; num < allDefsListForReading2.Count; num++)
					{
						ThingDef thingDef2 = allDefsListForReading2[num];
						if (thingDef2.tradeTags != null && thingDef2.tradeTags.Contains(this.tradeTagsToDisallow[n]))
						{
							this.SetAllow(thingDef2, false);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToAllow != null)
			{
				for (int num2 = 0; num2 < this.thingSetMakerTagsToAllow.Count; num2++)
				{
					List<ThingDef> allDefsListForReading3 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num3 = 0; num3 < allDefsListForReading3.Count; num3++)
					{
						ThingDef thingDef3 = allDefsListForReading3[num3];
						if (thingDef3.thingSetMakerTags != null && thingDef3.thingSetMakerTags.Contains(this.thingSetMakerTagsToAllow[num2]))
						{
							this.SetAllow(thingDef3, true);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToDisallow != null)
			{
				for (int num4 = 0; num4 < this.thingSetMakerTagsToDisallow.Count; num4++)
				{
					List<ThingDef> allDefsListForReading4 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num5 = 0; num5 < allDefsListForReading4.Count; num5++)
					{
						ThingDef thingDef4 = allDefsListForReading4[num5];
						if (thingDef4.thingSetMakerTags != null && thingDef4.thingSetMakerTags.Contains(this.thingSetMakerTagsToDisallow[num4]))
						{
							this.SetAllow(thingDef4, false);
						}
					}
				}
			}
			if (this.disallowedCategories != null)
			{
				for (int num6 = 0; num6 < this.disallowedCategories.Count; num6++)
				{
					ThingCategoryDef named2 = DefDatabase<ThingCategoryDef>.GetNamed(this.disallowedCategories[num6], true);
					if (named2 != null)
					{
						this.SetAllow(named2, false, null, null);
					}
				}
			}
			if (this.specialFiltersToAllow != null)
			{
				for (int num7 = 0; num7 < this.specialFiltersToAllow.Count; num7++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToAllow[num7]), true);
				}
			}
			if (this.specialFiltersToDisallow != null)
			{
				for (int num8 = 0; num8 < this.specialFiltersToDisallow.Count; num8++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToDisallow[num8]), false);
				}
			}
			if (this.stuffCategoriesToAllow != null)
			{
				for (int num9 = 0; num9 < this.stuffCategoriesToAllow.Count; num9++)
				{
					this.SetAllow(this.stuffCategoriesToAllow[num9], true);
				}
			}
			if (this.allowAllWhoCanMake != null)
			{
				for (int num10 = 0; num10 < this.allowAllWhoCanMake.Count; num10++)
				{
					this.SetAllowAllWhoCanMake(this.allowAllWhoCanMake[num10]);
				}
			}
			if (this.disallowWorsePreferability != FoodPreferability.Undefined)
			{
				List<ThingDef> allDefsListForReading5 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num11 = 0; num11 < allDefsListForReading5.Count; num11++)
				{
					ThingDef thingDef5 = allDefsListForReading5[num11];
					if (thingDef5.IsIngestible && thingDef5.ingestible.preferability != FoodPreferability.Undefined && thingDef5.ingestible.preferability < this.disallowWorsePreferability)
					{
						this.SetAllow(thingDef5, false);
					}
				}
			}
			if (this.disallowInedibleByHuman)
			{
				List<ThingDef> allDefsListForReading6 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num12 = 0; num12 < allDefsListForReading6.Count; num12++)
				{
					ThingDef thingDef6 = allDefsListForReading6[num12];
					if (thingDef6.IsIngestible && !ThingDefOf.Human.race.CanEverEat(thingDef6))
					{
						this.SetAllow(thingDef6, false);
					}
				}
			}
			if (this.allowWithComp != null)
			{
				List<ThingDef> allDefsListForReading7 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num13 = 0; num13 < allDefsListForReading7.Count; num13++)
				{
					ThingDef thingDef7 = allDefsListForReading7[num13];
					if (thingDef7.HasComp(this.allowWithComp))
					{
						this.SetAllow(thingDef7, true);
					}
				}
			}
			if (this.disallowWithComp != null)
			{
				List<ThingDef> allDefsListForReading8 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading8.Count; num14++)
				{
					ThingDef thingDef8 = allDefsListForReading8[num14];
					if (thingDef8.HasComp(this.disallowWithComp))
					{
						this.SetAllow(thingDef8, false);
					}
				}
			}
			if (this.disallowCheaperThan != -3.4028235E+38f)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef9 in this.allowedDefs)
				{
					if (thingDef9.BaseMarketValue < this.disallowCheaperThan)
					{
						list.Add(thingDef9);
					}
				}
				for (int num15 = 0; num15 < list.Count; num15++)
				{
					this.SetAllow(list[num15], false);
				}
			}
			if (this.disallowedThingDefs != null)
			{
				for (int num16 = 0; num16 < this.disallowedThingDefs.Count; num16++)
				{
					if (this.disallowedThingDefs[num16] != null)
					{
						this.SetAllow(this.disallowedThingDefs[num16], false);
					}
					else
					{
						Log.Error("ThingFilter could not find excepted thing def named " + this.disallowedThingDefs[num16], false);
					}
				}
			}
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x0015DBA4 File Offset: 0x0015BDA4
		public void RecalculateDisplayRootCategory()
		{
			if (ThingCategoryNodeDatabase.allThingCategoryNodes == null)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			int lastFoundCategory = -1;
			object lockObject = new object();
			GenThreading.ParallelFor(0, ThingCategoryNodeDatabase.allThingCategoryNodes.Count, delegate(int index)
			{
				TreeNode_ThingCategory treeNode_ThingCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[index];
				bool flag = false;
				bool flag2 = false;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (treeNode_ThingCategory.catDef.ContainedInThisOrDescendant(thingDef))
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
				if (!flag && flag2)
				{
					object lockObject = lockObject;
					lock (lockObject)
					{
						if (index > lastFoundCategory)
						{
							lastFoundCategory = index;
						}
					}
				}
			}, -1);
			if (lastFoundCategory == -1)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			this.DisplayRootCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[lastFoundCategory];
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x0015DC2C File Offset: 0x0015BE2C
		private void RecalculateSpecialFilterConfigurability()
		{
			if (this.DisplayRootCategory == null)
			{
				this.allowedHitPointsConfigurable = true;
				this.allowedQualitiesConfigurable = true;
				return;
			}
			this.allowedHitPointsConfigurable = false;
			this.allowedQualitiesConfigurable = false;
			foreach (ThingDef thingDef in this.DisplayRootCategory.catDef.DescendantThingDefs)
			{
				if (thingDef.useHitPoints)
				{
					this.allowedHitPointsConfigurable = true;
				}
				if (thingDef.HasComp(typeof(CompQuality)))
				{
					this.allowedQualitiesConfigurable = true;
				}
				if (this.allowedHitPointsConfigurable && this.allowedQualitiesConfigurable)
				{
					break;
				}
			}
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x0015DCDC File Offset: 0x0015BEDC
		public bool IsAlwaysDisallowedDueToSpecialFilters(ThingDef def)
		{
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.AlwaysMatches(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x0015DD1C File Offset: 0x0015BF1C
		public virtual void CopyAllowancesFrom(ThingFilter other)
		{
			this.allowedDefs.Clear();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				this.SetAllow(thingDef, other.Allows(thingDef));
			}
			this.disallowedSpecialFilters = other.disallowedSpecialFilters.ListFullCopyOrNull<SpecialThingFilterDef>();
			this.allowedHitPointsPercents = other.allowedHitPointsPercents;
			this.allowedHitPointsConfigurable = other.allowedHitPointsConfigurable;
			this.allowedQualities = other.allowedQualities;
			this.allowedQualitiesConfigurable = other.allowedQualitiesConfigurable;
			this.thingDefs = other.thingDefs.ListFullCopyOrNull<ThingDef>();
			this.categories = other.categories.ListFullCopyOrNull<string>();
			this.tradeTagsToAllow = other.tradeTagsToAllow.ListFullCopyOrNull<string>();
			this.tradeTagsToDisallow = other.tradeTagsToDisallow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToAllow = other.thingSetMakerTagsToAllow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToDisallow = other.thingSetMakerTagsToDisallow.ListFullCopyOrNull<string>();
			this.disallowedCategories = other.disallowedCategories.ListFullCopyOrNull<string>();
			this.specialFiltersToAllow = other.specialFiltersToAllow.ListFullCopyOrNull<string>();
			this.specialFiltersToDisallow = other.specialFiltersToDisallow.ListFullCopyOrNull<string>();
			this.stuffCategoriesToAllow = other.stuffCategoriesToAllow.ListFullCopyOrNull<StuffCategoryDef>();
			this.allowAllWhoCanMake = other.allowAllWhoCanMake.ListFullCopyOrNull<ThingDef>();
			this.disallowWorsePreferability = other.disallowWorsePreferability;
			this.disallowInedibleByHuman = other.disallowInedibleByHuman;
			this.allowWithComp = other.allowWithComp;
			this.disallowWithComp = other.disallowWithComp;
			this.disallowCheaperThan = other.disallowCheaperThan;
			this.disallowedThingDefs = other.disallowedThingDefs.ListFullCopyOrNull<ThingDef>();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x0015DED8 File Offset: 0x0015C0D8
		public void SetAllow(ThingDef thingDef, bool allow)
		{
			if (allow == this.Allows(thingDef))
			{
				return;
			}
			if (allow)
			{
				this.allowedDefs.Add(thingDef);
			}
			else
			{
				this.allowedDefs.Remove(thingDef);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x0015DF2C File Offset: 0x0015C12C
		public void SetAllow(SpecialThingFilterDef sfDef, bool allow)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			if (allow == this.Allows(sfDef))
			{
				return;
			}
			if (allow)
			{
				if (this.disallowedSpecialFilters.Contains(sfDef))
				{
					this.disallowedSpecialFilters.Remove(sfDef);
				}
			}
			else if (!this.disallowedSpecialFilters.Contains(sfDef))
			{
				this.disallowedSpecialFilters.Add(sfDef);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x0015DFA4 File Offset: 0x0015C1A4
		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.", false);
			}
			foreach (ThingDef thingDef in categoryDef.DescendantThingDefs)
			{
				if (exceptedDefs == null || !exceptedDefs.Contains(thingDef))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in categoryDef.DescendantSpecialThingFilterDefs)
			{
				if (exceptedFilters == null || !exceptedFilters.Contains(specialThingFilterDef))
				{
					this.SetAllow(specialThingFilterDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x0015E078 File Offset: 0x0015C278
		public void SetAllow(StuffCategoryDef cat, bool allow)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.categories.Contains(cat))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x0015E0E4 File Offset: 0x0015C2E4
		public void SetAllowAllWhoCanMake(ThingDef thing)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.CanMake(thing))
				{
					this.SetAllow(thingDef, true);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x0015E14C File Offset: 0x0015C34C
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Manufactured, true, null, null);
				this.SetAllow(ThingCategoryDefOf.ResourcesRaw, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Items, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Buildings, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Weapons, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				this.SetAllow(ThingCategoryDefOf.BodyParts, true, null, null);
			}
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Chunks, true, null, null);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x0015E208 File Offset: 0x0015C408
		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x0015E274 File Offset: 0x0015C474
		public void SetAllowAll(ThingFilter parentFilter, bool includeNonStorable = false)
		{
			this.allowedDefs.Clear();
			if (parentFilter != null)
			{
				using (HashSet<ThingDef>.Enumerator enumerator = parentFilter.allowedDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef item = enumerator.Current;
						this.allowedDefs.Add(item);
					}
					goto IL_B9;
				}
			}
			if (includeNonStorable)
			{
				using (IEnumerator<ThingDef> enumerator2 = DefDatabase<ThingDef>.AllDefs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDef item2 = enumerator2.Current;
						this.allowedDefs.Add(item2);
					}
					goto IL_B9;
				}
			}
			foreach (ThingDef item3 in ThingFilter.AllStorableThingDefs)
			{
				this.allowedDefs.Add(item3);
			}
			IL_B9:
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable);
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x0015E3A8 File Offset: 0x0015C5A8
		public virtual bool Allows(Thing t)
		{
			t = t.GetInnerIfMinified();
			if (!this.Allows(t.def))
			{
				return false;
			}
			if (t.def.useHitPoints)
			{
				float num = (float)t.HitPoints / (float)t.MaxHitPoints;
				num = GenMath.RoundedHundredth(num);
				if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(num)))
				{
					return false;
				}
			}
			if (this.allowedQualities != QualityRange.All && t.def.FollowQualityThingFilter())
			{
				QualityCategory p;
				if (!t.TryGetQuality(out p))
				{
					p = QualityCategory.Normal;
				}
				if (!this.allowedQualities.Includes(p))
				{
					return false;
				}
			}
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.Matches(t) && t.def.IsWithinCategory(this.disallowedSpecialFilters[i].parentCategory))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x0002A7F8 File Offset: 0x000289F8
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x0002A806 File Offset: 0x00028A06
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x0002A817 File Offset: 0x00028A17
		public ThingRequest GetThingRequest()
		{
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			return ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x0002A852 File Offset: 0x00028A52
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x04002631 RID: 9777
		[Unsaved(false)]
		private Action settingsChangedCallback;

		// Token: 0x04002632 RID: 9778
		[Unsaved(false)]
		private TreeNode_ThingCategory displayRootCategoryInt;

		// Token: 0x04002633 RID: 9779
		[Unsaved(false)]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x04002634 RID: 9780
		[Unsaved(false)]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04002635 RID: 9781
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x04002636 RID: 9782
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x04002637 RID: 9783
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x04002638 RID: 9784
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x04002639 RID: 9785
		[MustTranslate]
		public string customSummary;

		// Token: 0x0400263A RID: 9786
		private List<ThingDef> thingDefs;

		// Token: 0x0400263B RID: 9787
		[NoTranslate]
		private List<string> categories;

		// Token: 0x0400263C RID: 9788
		[NoTranslate]
		private List<string> tradeTagsToAllow;

		// Token: 0x0400263D RID: 9789
		[NoTranslate]
		private List<string> tradeTagsToDisallow;

		// Token: 0x0400263E RID: 9790
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow;

		// Token: 0x0400263F RID: 9791
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow;

		// Token: 0x04002640 RID: 9792
		[NoTranslate]
		private List<string> disallowedCategories;

		// Token: 0x04002641 RID: 9793
		[NoTranslate]
		private List<string> specialFiltersToAllow;

		// Token: 0x04002642 RID: 9794
		[NoTranslate]
		private List<string> specialFiltersToDisallow;

		// Token: 0x04002643 RID: 9795
		private List<StuffCategoryDef> stuffCategoriesToAllow;

		// Token: 0x04002644 RID: 9796
		private List<ThingDef> allowAllWhoCanMake;

		// Token: 0x04002645 RID: 9797
		private FoodPreferability disallowWorsePreferability;

		// Token: 0x04002646 RID: 9798
		private bool disallowInedibleByHuman;

		// Token: 0x04002647 RID: 9799
		private Type allowWithComp;

		// Token: 0x04002648 RID: 9800
		private Type disallowWithComp;

		// Token: 0x04002649 RID: 9801
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x0400264A RID: 9802
		private List<ThingDef> disallowedThingDefs;
	}
}
