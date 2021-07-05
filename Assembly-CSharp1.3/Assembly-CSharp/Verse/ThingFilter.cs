using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EF RID: 1263
	public class ThingFilter : IExposable
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x000EBFC0 File Offset: 0x000EA1C0
		public string Summary
		{
			get
			{
				if (!this.customSummary.NullOrEmpty())
				{
					return this.customSummary;
				}
				if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty<string>() && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && !this.disallowNotEverStorable && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return this.thingDefs[0].label;
				}
				if (this.thingDefs.NullOrEmpty<ThingDef>() && this.categories != null && this.categories.Count == 1 && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && !this.disallowNotEverStorable && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
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

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x000EC240 File Offset: 0x000EA440
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

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x000EC2E0 File Offset: 0x000EA4E0
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x000EC2ED File Offset: 0x000EA4ED
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x000EC2F5 File Offset: 0x000EA4F5
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x000EC320 File Offset: 0x000EA520
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x000EC32D File Offset: 0x000EA52D
		// (set) Token: 0x06002618 RID: 9752 RVA: 0x000EC335 File Offset: 0x000EA535
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

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x000EC360 File Offset: 0x000EA560
		// (set) Token: 0x0600261A RID: 9754 RVA: 0x000EC368 File Offset: 0x000EA568
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

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x000EC393 File Offset: 0x000EA593
		// (set) Token: 0x0600261C RID: 9756 RVA: 0x000EC3B7 File Offset: 0x000EA5B7
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

		// Token: 0x0600261D RID: 9757 RVA: 0x000EC3D0 File Offset: 0x000EA5D0
		public ThingFilter()
		{
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000EC428 File Offset: 0x000EA628
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000EC488 File Offset: 0x000EA688
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000EC4F0 File Offset: 0x000EA6F0
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
						Log.Error("ThingFilter could not find thing def named " + this.thingDefs[j]);
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
			if (this.disallowNotEverStorable)
			{
				foreach (ThingDef thingDef7 in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (!thingDef7.EverStorable(true))
					{
						this.SetAllow(thingDef7, false);
					}
				}
			}
			if (this.allowWithComp != null)
			{
				List<ThingDef> allDefsListForReading7 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num13 = 0; num13 < allDefsListForReading7.Count; num13++)
				{
					ThingDef thingDef8 = allDefsListForReading7[num13];
					if (thingDef8.HasComp(this.allowWithComp))
					{
						this.SetAllow(thingDef8, true);
					}
				}
			}
			if (this.disallowWithComp != null)
			{
				List<ThingDef> allDefsListForReading8 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading8.Count; num14++)
				{
					ThingDef thingDef9 = allDefsListForReading8[num14];
					if (thingDef9.HasComp(this.disallowWithComp))
					{
						this.SetAllow(thingDef9, false);
					}
				}
			}
			if (this.disallowCheaperThan != -3.4028235E+38f)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef10 in this.allowedDefs)
				{
					if (thingDef10.BaseMarketValue < this.disallowCheaperThan)
					{
						list.Add(thingDef10);
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
						Log.Error("ThingFilter could not find excepted thing def named " + this.disallowedThingDefs[num16]);
					}
				}
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000ECB70 File Offset: 0x000EAD70
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

		// Token: 0x06002622 RID: 9762 RVA: 0x000ECBF8 File Offset: 0x000EADF8
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

		// Token: 0x06002623 RID: 9763 RVA: 0x000ECCA8 File Offset: 0x000EAEA8
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

		// Token: 0x06002624 RID: 9764 RVA: 0x000ECCE8 File Offset: 0x000EAEE8
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
			this.disallowNotEverStorable = other.disallowNotEverStorable;
			this.allowWithComp = other.allowWithComp;
			this.disallowWithComp = other.disallowWithComp;
			this.disallowCheaperThan = other.disallowCheaperThan;
			this.disallowedThingDefs = other.disallowedThingDefs.ListFullCopyOrNull<ThingDef>();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000ECEB0 File Offset: 0x000EB0B0
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

		// Token: 0x06002626 RID: 9766 RVA: 0x000ECF04 File Offset: 0x000EB104
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

		// Token: 0x06002627 RID: 9767 RVA: 0x000ECF7C File Offset: 0x000EB17C
		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.");
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

		// Token: 0x06002628 RID: 9768 RVA: 0x000ED04C File Offset: 0x000EB24C
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

		// Token: 0x06002629 RID: 9769 RVA: 0x000ED0B8 File Offset: 0x000EB2B8
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

		// Token: 0x0600262A RID: 9770 RVA: 0x000ED120 File Offset: 0x000EB320
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

		// Token: 0x0600262B RID: 9771 RVA: 0x000ED1DC File Offset: 0x000EB3DC
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

		// Token: 0x0600262C RID: 9772 RVA: 0x000ED248 File Offset: 0x000EB448
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

		// Token: 0x0600262D RID: 9773 RVA: 0x000ED37C File Offset: 0x000EB57C
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

		// Token: 0x0600262E RID: 9774 RVA: 0x000ED465 File Offset: 0x000EB665
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000ED473 File Offset: 0x000EB673
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000ED484 File Offset: 0x000EB684
		public ThingRequest GetThingRequest()
		{
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			return ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000ED4BF File Offset: 0x000EB6BF
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000ED4C7 File Offset: 0x000EB6C7
		public static ThingFilter CreateOnlyEverStorableThingFilter()
		{
			ThingFilter thingFilter = new ThingFilter();
			thingFilter.categories = new List<string>();
			thingFilter.categories.Add(ThingCategoryDefOf.Root.defName);
			thingFilter.disallowNotEverStorable = true;
			thingFilter.ResolveReferences();
			return thingFilter;
		}

		// Token: 0x040017EB RID: 6123
		[Unsaved(false)]
		private Action settingsChangedCallback;

		// Token: 0x040017EC RID: 6124
		[Unsaved(false)]
		private TreeNode_ThingCategory displayRootCategoryInt;

		// Token: 0x040017ED RID: 6125
		[Unsaved(false)]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x040017EE RID: 6126
		[Unsaved(false)]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x040017EF RID: 6127
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x040017F0 RID: 6128
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x040017F1 RID: 6129
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x040017F2 RID: 6130
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x040017F3 RID: 6131
		[MustTranslate]
		public string customSummary;

		// Token: 0x040017F4 RID: 6132
		private List<ThingDef> thingDefs;

		// Token: 0x040017F5 RID: 6133
		[NoTranslate]
		private List<string> categories;

		// Token: 0x040017F6 RID: 6134
		[NoTranslate]
		private List<string> tradeTagsToAllow;

		// Token: 0x040017F7 RID: 6135
		[NoTranslate]
		private List<string> tradeTagsToDisallow;

		// Token: 0x040017F8 RID: 6136
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow;

		// Token: 0x040017F9 RID: 6137
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow;

		// Token: 0x040017FA RID: 6138
		[NoTranslate]
		private List<string> disallowedCategories;

		// Token: 0x040017FB RID: 6139
		[NoTranslate]
		private List<string> specialFiltersToAllow;

		// Token: 0x040017FC RID: 6140
		[NoTranslate]
		private List<string> specialFiltersToDisallow;

		// Token: 0x040017FD RID: 6141
		private List<StuffCategoryDef> stuffCategoriesToAllow;

		// Token: 0x040017FE RID: 6142
		private List<ThingDef> allowAllWhoCanMake;

		// Token: 0x040017FF RID: 6143
		private FoodPreferability disallowWorsePreferability;

		// Token: 0x04001800 RID: 6144
		private bool disallowInedibleByHuman;

		// Token: 0x04001801 RID: 6145
		private bool disallowNotEverStorable;

		// Token: 0x04001802 RID: 6146
		private Type allowWithComp;

		// Token: 0x04001803 RID: 6147
		private Type disallowWithComp;

		// Token: 0x04001804 RID: 6148
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x04001805 RID: 6149
		private List<ThingDef> disallowedThingDefs;
	}
}
