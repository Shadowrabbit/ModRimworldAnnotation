using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001321 RID: 4897
	public class Page_CreateWorldParams : Page
	{
		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x06007643 RID: 30275 RVA: 0x0028E874 File Offset: 0x0028CA74
		public override string PageTitle
		{
			get
			{
				return "CreateWorld".Translate();
			}
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x0028E885 File Offset: 0x0028CA85
		public override void PreOpen()
		{
			base.PreOpen();
			if (!this.initialized)
			{
				this.Reset();
				this.initialized = true;
			}
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x0028E8A2 File Offset: 0x0028CAA2
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-CreateWorldParams");
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x0028E8BC File Offset: 0x0028CABC
		public void Reset()
		{
			this.seedString = GenText.RandomSeedString();
			this.planetCoverage = ((!Prefs.DevMode || !UnityData.isEditor) ? 0.3f : 0.05f);
			this.rainfall = OverallRainfall.Normal;
			this.temperature = OverallTemperature.Normal;
			this.population = OverallPopulation.Normal;
			this.ResetFactionCounts();
		}

		// Token: 0x06007647 RID: 30279 RVA: 0x0028E910 File Offset: 0x0028CB10
		private void ResetFactionCounts()
		{
			this.factionCounts = new Dictionary<FactionDef, int>();
			foreach (FactionDef factionDef in FactionGenerator.ConfigurableFactions)
			{
				this.factionCounts.Add(factionDef, factionDef.startingCountAtWorldCreation);
			}
		}

		// Token: 0x06007648 RID: 30280 RVA: 0x0028E974 File Offset: 0x0028CB74
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			float width = mainRect.width * 0.5f - 2f * this.Margin;
			Rect position = new Rect(mainRect.x, mainRect.y, width, mainRect.height);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			float num = 0f;
			float width2 = position.width - 200f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "WorldSeed".Translate());
			Rect rect2 = new Rect(200f, num, width2, 30f);
			this.seedString = Widgets.TextField(rect2, this.seedString);
			num += 40f;
			if (Widgets.ButtonText(new Rect(200f, num, width2, 30f), "RandomizeSeed".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.seedString = GenText.RandomSeedString();
			}
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetCoverage".Translate());
			Rect rect3 = new Rect(200f, num, width2, 30f);
			if (Widgets.ButtonText(rect3, this.planetCoverage.ToStringPercent(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				float[] array = Prefs.DevMode ? Page_CreateWorldParams.PlanetCoveragesDev : Page_CreateWorldParams.PlanetCoverages;
				for (int i = 0; i < array.Length; i++)
				{
					float coverage = array[i];
					string text = coverage.ToStringPercent();
					if (coverage <= 0.1f)
					{
						text += " (dev)";
					}
					FloatMenuOption item = new FloatMenuOption(text, delegate()
					{
						if (this.planetCoverage != coverage)
						{
							this.planetCoverage = coverage;
							if (this.planetCoverage == 1f)
							{
								Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageTypeDefOf.CautionInput, false);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			TooltipHandler.TipRegionByKey(new Rect(0f, num, rect3.xMax, rect3.height), "PlanetCoverageTip");
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetRainfall".Translate());
			Rect rect4 = new Rect(200f, num, width2, 30f);
			this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect4, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetTemperature".Translate());
			Rect rect5 = new Rect(200f, num, width2, 30f);
			this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetPopulation".Translate());
			Rect rect6 = new Rect(200f, num, width2, 30f);
			this.population = (OverallPopulation)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)this.population, 0f, (float)(OverallPopulationUtility.EnumValuesCount - 1), true, "PlanetPopulation_Normal".Translate(), "PlanetPopulation_Low".Translate(), "PlanetPopulation_High".Translate(), 1f));
			GUI.EndGroup();
			Rect rect7 = new Rect(mainRect.x + mainRect.width / 2f, mainRect.y, width, mainRect.height);
			IEnumerable<FactionDef> configurableFactions = FactionGenerator.ConfigurableFactions;
			bool flag = true;
			foreach (FactionDef factionDef in configurableFactions)
			{
				if (flag && this.factionCounts[factionDef] != factionDef.startingCountAtWorldCreation)
				{
					flag = false;
				}
			}
			bool flag2;
			WorldFactionsUIUtility.DoWindowContents(rect7, ref this.factionCounts, flag, out flag2);
			if (flag2)
			{
				this.ResetFactionCounts();
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			base.DoBottomButtons(rect, "WorldGenerate".Translate(), "Reset".Translate(), new Action(this.Reset), true, true);
		}

		// Token: 0x06007649 RID: 30281 RVA: 0x0028EE78 File Offset: 0x0028D078
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.GameInitData.ResetWorldRelatedMapInitData();
				Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature, this.population, this.factionCounts);
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.next != null)
					{
						Find.WindowStack.Add(this.next);
					}
					MemoryUtility.UnloadUnusedUnityAssets();
					Find.World.renderer.RegenerateAllLayersNow();
					this.Close(true);
				});
			}, "GeneratingWorld", true, null, true);
			return false;
		}

		// Token: 0x04004197 RID: 16791
		private bool initialized;

		// Token: 0x04004198 RID: 16792
		private string seedString;

		// Token: 0x04004199 RID: 16793
		private float planetCoverage;

		// Token: 0x0400419A RID: 16794
		private OverallRainfall rainfall;

		// Token: 0x0400419B RID: 16795
		private OverallTemperature temperature;

		// Token: 0x0400419C RID: 16796
		private OverallPopulation population;

		// Token: 0x0400419D RID: 16797
		private Dictionary<FactionDef, int> factionCounts;

		// Token: 0x0400419E RID: 16798
		private static readonly float[] PlanetCoverages = new float[]
		{
			0.3f,
			0.5f,
			1f
		};

		// Token: 0x0400419F RID: 16799
		private static readonly float[] PlanetCoveragesDev = new float[]
		{
			0.3f,
			0.5f,
			1f,
			0.05f
		};

		// Token: 0x040041A0 RID: 16800
		private const float LabelWidth = 200f;
	}
}
