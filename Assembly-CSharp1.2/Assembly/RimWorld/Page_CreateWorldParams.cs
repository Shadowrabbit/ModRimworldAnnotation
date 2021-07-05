using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A93 RID: 6803
	public class Page_CreateWorldParams : Page
	{
		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x06009646 RID: 38470 RVA: 0x000644F6 File Offset: 0x000626F6
		public override string PageTitle
		{
			get
			{
				return "CreateWorld".Translate();
			}
		}

		// Token: 0x06009647 RID: 38471 RVA: 0x00064507 File Offset: 0x00062707
		public override void PreOpen()
		{
			base.PreOpen();
			if (!this.initialized)
			{
				this.Reset();
				this.initialized = true;
			}
		}

		// Token: 0x06009648 RID: 38472 RVA: 0x00064524 File Offset: 0x00062724
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-CreateWorldParams");
		}

		// Token: 0x06009649 RID: 38473 RVA: 0x002BAC1C File Offset: 0x002B8E1C
		public void Reset()
		{
			this.seedString = GenText.RandomSeedString();
			this.planetCoverage = ((!Prefs.DevMode || !UnityData.isEditor) ? 0.3f : 0.05f);
			this.rainfall = OverallRainfall.Normal;
			this.temperature = OverallTemperature.Normal;
			this.population = OverallPopulation.Normal;
		}

		// Token: 0x0600964A RID: 38474 RVA: 0x002BAC6C File Offset: 0x002B8E6C
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			GUI.BeginGroup(base.GetMainRect(rect, 0f, false));
			Text.Font = GameFont.Small;
			float num = 0f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "WorldSeed".Translate());
			Rect rect2 = new Rect(200f, num, 200f, 30f);
			this.seedString = Widgets.TextField(rect2, this.seedString);
			num += 40f;
			if (Widgets.ButtonText(new Rect(200f, num, 200f, 30f), "RandomizeSeed".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.seedString = GenText.RandomSeedString();
			}
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetCoverage".Translate());
			Rect rect3 = new Rect(200f, num, 200f, 30f);
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			TooltipHandler.TipRegionByKey(new Rect(0f, num, rect3.xMax, rect3.height), "PlanetCoverageTip");
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetRainfall".Translate());
			Rect rect4 = new Rect(200f, num, 200f, 30f);
			this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect4, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetTemperature".Translate());
			Rect rect5 = new Rect(200f, num, 200f, 30f);
			this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetPopulation".Translate());
			Rect rect6 = new Rect(200f, num, 200f, 30f);
			this.population = (OverallPopulation)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)this.population, 0f, (float)(OverallPopulationUtility.EnumValuesCount - 1), true, "PlanetPopulation_Normal".Translate(), "PlanetPopulation_Low".Translate(), "PlanetPopulation_High".Translate(), 1f));
			GUI.EndGroup();
			base.DoBottomButtons(rect, "WorldGenerate".Translate(), "Reset".Translate(), new Action(this.Reset), true, true);
		}

		// Token: 0x0600964B RID: 38475 RVA: 0x0006453B File Offset: 0x0006273B
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.GameInitData.ResetWorldRelatedMapInitData();
				Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature, this.population);
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

		// Token: 0x04005FC8 RID: 24520
		private bool initialized;

		// Token: 0x04005FC9 RID: 24521
		private string seedString;

		// Token: 0x04005FCA RID: 24522
		private float planetCoverage;

		// Token: 0x04005FCB RID: 24523
		private OverallRainfall rainfall;

		// Token: 0x04005FCC RID: 24524
		private OverallTemperature temperature;

		// Token: 0x04005FCD RID: 24525
		private OverallPopulation population;

		// Token: 0x04005FCE RID: 24526
		private static readonly float[] PlanetCoverages = new float[]
		{
			0.3f,
			0.5f,
			1f
		};

		// Token: 0x04005FCF RID: 24527
		private static readonly float[] PlanetCoveragesDev = new float[]
		{
			0.3f,
			0.5f,
			1f,
			0.05f
		};
	}
}
