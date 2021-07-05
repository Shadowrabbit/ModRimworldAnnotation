using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A30 RID: 2608
	public class PlantProperties
	{
		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003F39 RID: 16185 RVA: 0x001587CC File Offset: 0x001569CC
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x001587DC File Offset: 0x001569DC
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003F3B RID: 16187 RVA: 0x001587EB File Offset: 0x001569EB
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003F3C RID: 16188 RVA: 0x001587FD File Offset: 0x001569FD
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood";
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003F3D RID: 16189 RVA: 0x0015880F File Offset: 0x00156A0F
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanDaysPerGrowDays;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x0015881E File Offset: 0x00156A1E
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003F3F RID: 16191 RVA: 0x0015882D File Offset: 0x00156A2D
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanDaysPerGrowDays > 0f;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x0015883C File Offset: 0x00156A3C
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003F41 RID: 16193 RVA: 0x00158859 File Offset: 0x00156A59
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x00158864 File Offset: 0x00156A64
		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x001588C1 File Offset: 0x00156AC1
		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x001588D1 File Offset: 0x00156AD1
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
		{
			yield return new Dialog_InfoCard.Hyperlink(this.harvestedThingDef, -1);
			yield break;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x001588E1 File Offset: 0x00156AE1
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
				stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.cropYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), Mathf.CeilToInt(this.harvestYield * Find.Storyteller.difficulty.cropYieldFactor).ToString("F0"), stringBuilder.ToString(), 4150, null, this.GetHarvestYieldHyperlinks(), false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowthTemperature".Translate(), 0f.ToStringTemperature("F1"), "Stat_Thing_Plant_MinGrowthTemperature_Desc".Translate(), 4152, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MaxGrowthTemperature".Translate(), 58f.ToStringTemperature("F1"), "Stat_Thing_Plant_MaxGrowthTemperature_Desc".Translate(), 4153, null, null, false);
			yield break;
		}

		// Token: 0x040022A1 RID: 8865
		public List<PlantBiomeRecord> wildBiomes;

		// Token: 0x040022A2 RID: 8866
		public int wildClusterRadius = -1;

		// Token: 0x040022A3 RID: 8867
		public float wildClusterWeight = 15f;

		// Token: 0x040022A4 RID: 8868
		public float wildOrder = 2f;

		// Token: 0x040022A5 RID: 8869
		public bool wildEqualLocalDistribution = true;

		// Token: 0x040022A6 RID: 8870
		public bool cavePlant;

		// Token: 0x040022A7 RID: 8871
		public float cavePlantWeight = 1f;

		// Token: 0x040022A8 RID: 8872
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		// Token: 0x040022A9 RID: 8873
		public float sowWork = 10f;

		// Token: 0x040022AA RID: 8874
		public int sowMinSkill;

		// Token: 0x040022AB RID: 8875
		public bool blockAdjacentSow;

		// Token: 0x040022AC RID: 8876
		public List<ResearchProjectDef> sowResearchPrerequisites;

		// Token: 0x040022AD RID: 8877
		public bool mustBeWildToSow;

		// Token: 0x040022AE RID: 8878
		public float harvestWork = 10f;

		// Token: 0x040022AF RID: 8879
		public float harvestYield;

		// Token: 0x040022B0 RID: 8880
		public ThingDef harvestedThingDef;

		// Token: 0x040022B1 RID: 8881
		[NoTranslate]
		public string harvestTag;

		// Token: 0x040022B2 RID: 8882
		public float harvestMinGrowth = 0.65f;

		// Token: 0x040022B3 RID: 8883
		public float harvestAfterGrowth;

		// Token: 0x040022B4 RID: 8884
		public bool harvestFailable = true;

		// Token: 0x040022B5 RID: 8885
		public SoundDef soundHarvesting;

		// Token: 0x040022B6 RID: 8886
		public SoundDef soundHarvestFinish;

		// Token: 0x040022B7 RID: 8887
		public float growDays = 2f;

		// Token: 0x040022B8 RID: 8888
		public float lifespanDaysPerGrowDays = 8f;

		// Token: 0x040022B9 RID: 8889
		public float growMinGlow = 0.51f;

		// Token: 0x040022BA RID: 8890
		public float growOptimalGlow = 1f;

		// Token: 0x040022BB RID: 8891
		public float fertilityMin = 0.9f;

		// Token: 0x040022BC RID: 8892
		public float fertilitySensitivity = 0.5f;

		// Token: 0x040022BD RID: 8893
		public bool dieIfLeafless;

		// Token: 0x040022BE RID: 8894
		public bool neverBlightable;

		// Token: 0x040022BF RID: 8895
		public bool interferesWithRoof;

		// Token: 0x040022C0 RID: 8896
		public bool dieIfNoSunlight = true;

		// Token: 0x040022C1 RID: 8897
		public bool dieFromToxicFallout = true;

		// Token: 0x040022C2 RID: 8898
		public PlantPurpose purpose = PlantPurpose.Misc;

		// Token: 0x040022C3 RID: 8899
		public bool humanFoodPlant;

		// Token: 0x040022C4 RID: 8900
		public bool treeLoversCareIfChopped = true;

		// Token: 0x040022C5 RID: 8901
		public bool allowAutoCut = true;

		// Token: 0x040022C6 RID: 8902
		public TreeCategory treeCategory;

		// Token: 0x040022C7 RID: 8903
		public ThingDef burnedThingDef;

		// Token: 0x040022C8 RID: 8904
		public bool showGrowthInInspectPane = true;

		// Token: 0x040022C9 RID: 8905
		public float minSpacingBetweenSamePlant;

		// Token: 0x040022CA RID: 8906
		public float topWindExposure = 0.25f;

		// Token: 0x040022CB RID: 8907
		public int maxMeshCount = 1;

		// Token: 0x040022CC RID: 8908
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x040022CD RID: 8909
		[NoTranslate]
		private string leaflessGraphicPath;

		// Token: 0x040022CE RID: 8910
		[Unsaved(false)]
		public Graphic leaflessGraphic;

		// Token: 0x040022CF RID: 8911
		[NoTranslate]
		private string immatureGraphicPath;

		// Token: 0x040022D0 RID: 8912
		[Unsaved(false)]
		public Graphic immatureGraphic;

		// Token: 0x040022D1 RID: 8913
		public bool dropLeaves;

		// Token: 0x040022D2 RID: 8914
		public const int MaxMaxMeshCount = 25;
	}
}
