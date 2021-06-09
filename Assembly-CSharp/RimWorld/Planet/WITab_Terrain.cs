using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021E2 RID: 8674
	public class WITab_Terrain : WITab
	{
		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x0600B9AF RID: 47535 RVA: 0x00078390 File Offset: 0x00076590
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		// Token: 0x0600B9B0 RID: 47536 RVA: 0x000783D2 File Offset: 0x000765D2
		public WITab_Terrain()
		{
			this.size = WITab_Terrain.WinSize;
			this.labelKey = "TabTerrain";
			this.tutorTag = "Terrain";
		}

		// Token: 0x0600B9B1 RID: 47537 RVA: 0x00356D5C File Offset: 0x00354F5C
		protected override void FillTab()
		{
			Rect outRect = new Rect(0f, 0f, WITab_Terrain.WinSize.x, WITab_Terrain.WinSize.y).ContractedBy(10f);
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, base.SelTile.biome.LabelCap);
			Rect rect3 = rect2;
			rect3.yMin += 35f;
			rect3.height = 99999f;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.verticalSpacing = 0f;
			listing_Standard.Begin(rect3);
			Tile selTile = base.SelTile;
			int selTileID = base.SelTileID;
			listing_Standard.Label(selTile.biome.description, -1f, null);
			listing_Standard.Gap(8f);
			listing_Standard.GapLine(12f);
			if (!selTile.biome.implemented)
			{
				listing_Standard.Label(selTile.biome.LabelCap + " " + "BiomeNotImplemented".Translate(), -1f, null);
			}
			listing_Standard.LabelDouble("Terrain".Translate(), selTile.hilliness.GetLabelCap(), null);
			if (selTile.Roads != null)
			{
				listing_Standard.LabelDouble("Road".Translate(), (from roadlink in selTile.Roads
				select roadlink.road.label).Distinct<string>().ToCommaList(true).CapitalizeFirst(), null);
			}
			if (selTile.Rivers != null)
			{
				listing_Standard.LabelDouble("River".Translate(), selTile.Rivers.MaxBy((Tile.RiverLink riverlink) => riverlink.river.degradeThreshold).river.LabelCap, null);
			}
			if (!Find.World.Impassable(selTileID))
			{
				StringBuilder stringBuilder = new StringBuilder();
				string rightLabel = (WorldPathGrid.CalculatedMovementDifficultyAt(selTileID, false, null, stringBuilder) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(selTileID, -1, stringBuilder)).ToString("0.#");
				if (WorldPathGrid.WillWinterEverAffectMovementDifficulty(selTileID) && WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(selTileID, null, null) < 2f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append(" (");
					stringBuilder.Append("MovementDifficultyOffsetInWinter".Translate("+" + 2f.ToString("0.#")));
					stringBuilder.Append(")");
				}
				listing_Standard.LabelDouble("MovementDifficulty".Translate(), rightLabel, stringBuilder.ToString());
			}
			if (selTile.biome.canBuildBase)
			{
				listing_Standard.LabelDouble("StoneTypesHere".Translate(), (from rt in Find.World.NaturalRockTypesIn(selTileID)
				select rt.label).ToCommaList(true).CapitalizeFirst(), null);
			}
			listing_Standard.LabelDouble("Elevation".Translate(), selTile.elevation.ToString("F0") + "m", null);
			listing_Standard.GapLine(12f);
			listing_Standard.LabelDouble("AvgTemp".Translate(), GenTemperature.GetAverageTemperatureLabel(selTileID), null);
			listing_Standard.LabelDouble("OutdoorGrowingPeriod".Translate(), Zone_Growing.GrowingQuadrumsDescription(selTileID), null);
			listing_Standard.LabelDouble("Rainfall".Translate(), selTile.rainfall.ToString("F0") + "mm", null);
			if (selTile.biome.foragedFood != null && selTile.biome.forageability > 0f)
			{
				listing_Standard.LabelDouble("Forageability".Translate(), selTile.biome.forageability.ToStringPercent() + " (" + selTile.biome.foragedFood.label + ")", null);
			}
			else
			{
				listing_Standard.LabelDouble("Forageability".Translate(), "0%", null);
			}
			listing_Standard.LabelDouble("AnimalsCanGrazeNow".Translate(), VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(selTileID) ? "Yes".Translate() : "No".Translate(), null);
			listing_Standard.GapLine(12f);
			listing_Standard.LabelDouble("AverageDiseaseFrequency".Translate(), string.Format("{0} {1}", (60f / selTile.biome.diseaseMtbDays).ToString("F1"), "PerYear".Translate()), null);
			listing_Standard.LabelDouble("TimeZone".Translate(), GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(selTileID).x).ToStringWithSign(), null);
			StringBuilder stringBuilder2 = new StringBuilder();
			Rot4 rot = Find.World.CoastDirectionAt(selTileID);
			if (rot.IsValid)
			{
				stringBuilder2.AppendWithComma(("HasCoast" + rot.ToString()).Translate());
			}
			if (Find.World.HasCaves(selTileID))
			{
				stringBuilder2.AppendWithComma("HasCaves".Translate());
			}
			if (stringBuilder2.Length > 0)
			{
				listing_Standard.LabelDouble("SpecialFeatures".Translate(), stringBuilder2.ToString().CapitalizeFirst(), null);
			}
			if (Prefs.DevMode)
			{
				listing_Standard.LabelDouble("Debug world tile ID", selTileID.ToString(), null);
			}
			this.lastDrawnHeight = rect3.y + listing_Standard.CurHeight;
			listing_Standard.End();
			Widgets.EndScrollView();
		}

		// Token: 0x04007EE3 RID: 32483
		private Vector2 scrollPosition;

		// Token: 0x04007EE4 RID: 32484
		private float lastDrawnHeight;

		// Token: 0x04007EE5 RID: 32485
		private static readonly Vector2 WinSize = new Vector2(440f, 540f);
	}
}
