using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F46 RID: 3910
	public class PlantProperties
	{
		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x0003BAE5 File Offset: 0x00039CE5
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060055F7 RID: 22007 RVA: 0x0003BAF5 File Offset: 0x00039CF5
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060055F8 RID: 22008 RVA: 0x0003BB04 File Offset: 0x00039D04
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060055F9 RID: 22009 RVA: 0x0003BB16 File Offset: 0x00039D16
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood";
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x0003BB28 File Offset: 0x00039D28
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanDaysPerGrowDays;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060055FB RID: 22011 RVA: 0x0003BB37 File Offset: 0x00039D37
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060055FC RID: 22012 RVA: 0x0003BB46 File Offset: 0x00039D46
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanDaysPerGrowDays > 0f;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060055FD RID: 22013 RVA: 0x0003BB55 File Offset: 0x00039D55
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060055FE RID: 22014 RVA: 0x0003BB72 File Offset: 0x00039D72
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x001C95B4 File Offset: 0x001C77B4
		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x0003BB7D File Offset: 0x00039D7D
		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x0003BB8D File Offset: 0x00039D8D
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
		{
			yield return new Dialog_InfoCard.Hyperlink(this.harvestedThingDef, -1);
			yield break;
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x0003BB9D File Offset: 0x00039D9D
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.sowMinSkill > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), this.sowMinSkill.ToString(), "Stat_Thing_Plant_MinGrowingSkillToSow_Desc".Translate(), 4151, null, null, false);
			}
			string attributes = "";
			if (this.Harvestable)
			{
				string text = "Harvestable".Translate();
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
					text = text.UncapitalizeFirst();
				}
				attributes += text;
			}
			if (this.LimitedLifespan)
			{
				string text2 = "LimitedLifespan".Translate();
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
					text2 = text2.UncapitalizeFirst();
				}
				attributes += text2;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), this.growDays.ToString("0.##") + " " + "Days".Translate(), "GrowingTimeDesc".Translate(), 4158, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilityRequirement".Translate(), this.fertilityMin.ToStringPercent(), "Stat_Thing_Plant_FertilityRequirement_Desc".Translate(), 4156, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilitySensitivity".Translate(), this.fertilitySensitivity.ToStringPercent(), "Stat_Thing_Plant_FertilitySensitivity_Desc".Translate(), 4155, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LightRequirement".Translate(), this.growMinGlow.ToStringPercent(), "Stat_Thing_Plant_LightRequirement_Desc".Translate(), 4154, null, null, false);
			if (!attributes.NullOrEmpty())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Attributes".Translate(), attributes, "Stat_Thing_Plant_Attributes_Desc".Translate(), 4157, null, null, false);
			}
			if (this.LimitedLifespan)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LifeSpan".Translate(), this.LifespanDays.ToString("0.##") + " " + "Days".Translate(), "Stat_Thing_Plant_LifeSpan_Desc".Translate(), 4150, null, null, false);
			}
			if (this.harvestYield > 0f)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_Thing_Plant_HarvestYield_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficultyValues.cropYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), Mathf.CeilToInt(this.harvestYield * Find.Storyteller.difficultyValues.cropYieldFactor).ToString("F0"), stringBuilder.ToString(), 4150, null, this.GetHarvestYieldHyperlinks(), false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowthTemperature".Translate(), 0f.ToStringTemperature("F1"), "Stat_Thing_Plant_MinGrowthTemperature_Desc".Translate(), 4152, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MaxGrowthTemperature".Translate(), 58f.ToStringTemperature("F1"), "Stat_Thing_Plant_MaxGrowthTemperature_Desc".Translate(), 4153, null, null, false);
			yield break;
		}

		// Token: 0x04003759 RID: 14169
		public List<PlantBiomeRecord> wildBiomes;

		// Token: 0x0400375A RID: 14170
		public int wildClusterRadius = -1;

		// Token: 0x0400375B RID: 14171
		public float wildClusterWeight = 15f;

		// Token: 0x0400375C RID: 14172
		public float wildOrder = 2f;

		// Token: 0x0400375D RID: 14173
		public bool wildEqualLocalDistribution = true;

		// Token: 0x0400375E RID: 14174
		public bool cavePlant;

		// Token: 0x0400375F RID: 14175
		public float cavePlantWeight = 1f;

		// Token: 0x04003760 RID: 14176
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		// Token: 0x04003761 RID: 14177
		public float sowWork = 10f;

		// Token: 0x04003762 RID: 14178
		public int sowMinSkill;

		// Token: 0x04003763 RID: 14179
		public bool blockAdjacentSow;

		// Token: 0x04003764 RID: 14180
		public List<ResearchProjectDef> sowResearchPrerequisites;

		// Token: 0x04003765 RID: 14181
		public bool mustBeWildToSow;

		// Token: 0x04003766 RID: 14182
		public float harvestWork = 10f;

		// Token: 0x04003767 RID: 14183
		public float harvestYield;

		// Token: 0x04003768 RID: 14184
		public ThingDef harvestedThingDef;

		// Token: 0x04003769 RID: 14185
		[NoTranslate]
		public string harvestTag;

		// Token: 0x0400376A RID: 14186
		public float harvestMinGrowth = 0.65f;

		// Token: 0x0400376B RID: 14187
		public float harvestAfterGrowth;

		// Token: 0x0400376C RID: 14188
		public bool harvestFailable = true;

		// Token: 0x0400376D RID: 14189
		public SoundDef soundHarvesting;

		// Token: 0x0400376E RID: 14190
		public SoundDef soundHarvestFinish;

		// Token: 0x0400376F RID: 14191
		public float growDays = 2f;

		// Token: 0x04003770 RID: 14192
		public float lifespanDaysPerGrowDays = 8f;

		// Token: 0x04003771 RID: 14193
		public float growMinGlow = 0.51f;

		// Token: 0x04003772 RID: 14194
		public float growOptimalGlow = 1f;

		// Token: 0x04003773 RID: 14195
		public float fertilityMin = 0.9f;

		// Token: 0x04003774 RID: 14196
		public float fertilitySensitivity = 0.5f;

		// Token: 0x04003775 RID: 14197
		public bool dieIfLeafless;

		// Token: 0x04003776 RID: 14198
		public bool neverBlightable;

		// Token: 0x04003777 RID: 14199
		public bool interferesWithRoof;

		// Token: 0x04003778 RID: 14200
		public bool dieIfNoSunlight = true;

		// Token: 0x04003779 RID: 14201
		public bool dieFromToxicFallout = true;

		// Token: 0x0400377A RID: 14202
		public PlantPurpose purpose = PlantPurpose.Misc;

		// Token: 0x0400377B RID: 14203
		public float topWindExposure = 0.25f;

		// Token: 0x0400377C RID: 14204
		public int maxMeshCount = 1;

		// Token: 0x0400377D RID: 14205
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x0400377E RID: 14206
		[NoTranslate]
		private string leaflessGraphicPath;

		// Token: 0x0400377F RID: 14207
		[Unsaved(false)]
		public Graphic leaflessGraphic;

		// Token: 0x04003780 RID: 14208
		[NoTranslate]
		private string immatureGraphicPath;

		// Token: 0x04003781 RID: 14209
		[Unsaved(false)]
		public Graphic immatureGraphic;

		// Token: 0x04003782 RID: 14210
		public bool dropLeaves;

		// Token: 0x04003783 RID: 14211
		public const int MaxMaxMeshCount = 25;
	}
}
