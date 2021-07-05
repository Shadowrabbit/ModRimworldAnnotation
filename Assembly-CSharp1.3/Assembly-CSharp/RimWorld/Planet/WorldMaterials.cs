using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200176C RID: 5996
	[StaticConstructorOnStartup]
	public static class WorldMaterials
	{
		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06008A51 RID: 35409 RVA: 0x00319FEE File Offset: 0x003181EE
		public static Material CurTargetingMat
		{
			get
			{
				WorldMaterials.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return WorldMaterials.TargetSquareMatSingle;
			}
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x0031A004 File Offset: 0x00318204
		static WorldMaterials()
		{
			WorldMaterials.GenerateMats(ref WorldMaterials.matsFertility, WorldMaterials.FertilitySpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsTemperature, WorldMaterials.TemperatureSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsElevation, WorldMaterials.ElevationSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsRainfall, WorldMaterials.RainfallSpectrum, WorldMaterials.NumMatsPerMode);
		}

		// Token: 0x06008A53 RID: 35411 RVA: 0x0031A550 File Offset: 0x00318750
		private static void GenerateMats(ref Material[] mats, Color[] colorSpectrum, int numMats)
		{
			mats = new Material[numMats];
			for (int i = 0; i < numMats; i++)
			{
				mats[i] = MatsFromSpectrum.Get(colorSpectrum, (float)i / (float)numMats);
			}
		}

		// Token: 0x06008A54 RID: 35412 RVA: 0x0031A580 File Offset: 0x00318780
		public static Material MatForFertilityOverlay(float fert)
		{
			int value = Mathf.FloorToInt(fert * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsFertility[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06008A55 RID: 35413 RVA: 0x0031A5B0 File Offset: 0x003187B0
		public static Material MatForTemperature(float temp)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(-50f, 50f, temp) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsTemperature[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06008A56 RID: 35414 RVA: 0x0031A5F0 File Offset: 0x003187F0
		public static Material MatForElevation(float elev)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, elev) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsElevation[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06008A57 RID: 35415 RVA: 0x0031A630 File Offset: 0x00318830
		public static Material MatForRainfallOverlay(float rain)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, rain) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsRainfall[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x040057E6 RID: 22502
		public static readonly Material WorldTerrain = MatLoader.LoadMat("World/WorldTerrain", 3500);

		// Token: 0x040057E7 RID: 22503
		public static readonly Material WorldIce = MatLoader.LoadMat("World/WorldIce", 3500);

		// Token: 0x040057E8 RID: 22504
		public static readonly Material WorldOcean = MatLoader.LoadMat("World/WorldOcean", 3500);

		// Token: 0x040057E9 RID: 22505
		public static readonly Material UngeneratedPlanetParts = MatLoader.LoadMat("World/UngeneratedPlanetParts", 3500);

		// Token: 0x040057EA RID: 22506
		public static readonly Material Rivers = MatLoader.LoadMat("World/Rivers", 3530);

		// Token: 0x040057EB RID: 22507
		public static readonly Material RiversBorder = MatLoader.LoadMat("World/RiversBorder", 3520);

		// Token: 0x040057EC RID: 22508
		public static readonly Material Roads = MatLoader.LoadMat("World/Roads", 3540);

		// Token: 0x040057ED RID: 22509
		public static int DebugTileRenderQueue = 3510;

		// Token: 0x040057EE RID: 22510
		public static int WorldObjectRenderQueue = 3550;

		// Token: 0x040057EF RID: 22511
		public static int WorldLineRenderQueue = 3590;

		// Token: 0x040057F0 RID: 22512
		public static int DynamicObjectRenderQueue = 3600;

		// Token: 0x040057F1 RID: 22513
		public static int FeatureNameRenderQueue = 3610;

		// Token: 0x040057F2 RID: 22514
		public static readonly Material MouseTile = MaterialPool.MatFrom("World/MouseTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x040057F3 RID: 22515
		public static readonly Material SelectedTile = MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x040057F4 RID: 22516
		public static readonly Material CurrentMapTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayTransparent, 3560);

		// Token: 0x040057F5 RID: 22517
		public static readonly Material Stars = MatLoader.LoadMat("World/Stars", -1);

		// Token: 0x040057F6 RID: 22518
		public static readonly Material Sun = MatLoader.LoadMat("World/Sun", -1);

		// Token: 0x040057F7 RID: 22519
		public static readonly Material PlanetGlow = MatLoader.LoadMat("World/PlanetGlow", -1);

		// Token: 0x040057F8 RID: 22520
		public static readonly Material SmallHills = MaterialPool.MatFrom("World/Hills/SmallHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x040057F9 RID: 22521
		public static readonly Material LargeHills = MaterialPool.MatFrom("World/Hills/LargeHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x040057FA RID: 22522
		public static readonly Material Mountains = MaterialPool.MatFrom("World/Hills/Mountains", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x040057FB RID: 22523
		public static readonly Material ImpassableMountains = MaterialPool.MatFrom("World/Hills/Impassable", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x040057FC RID: 22524
		public static readonly Material VertexColor = MatLoader.LoadMat("World/WorldVertexColor", -1);

		// Token: 0x040057FD RID: 22525
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, 3560);

		// Token: 0x040057FE RID: 22526
		private static int NumMatsPerMode = 50;

		// Token: 0x040057FF RID: 22527
		public static Material OverlayModeMatOcean = SolidColorMaterials.NewSolidColorMaterial(new Color(0.09f, 0.18f, 0.2f), ShaderDatabase.Transparent);

		// Token: 0x04005800 RID: 22528
		private static Material[] matsFertility;

		// Token: 0x04005801 RID: 22529
		private static readonly Color[] FertilitySpectrum = new Color[]
		{
			new Color(0f, 1f, 0f, 0f),
			new Color(0f, 1f, 0f, 0.5f)
		};

		// Token: 0x04005802 RID: 22530
		private const float TempRange = 50f;

		// Token: 0x04005803 RID: 22531
		private static Material[] matsTemperature;

		// Token: 0x04005804 RID: 22532
		private static readonly Color[] TemperatureSpectrum = new Color[]
		{
			new Color(1f, 1f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.25f, 0.25f, 1f),
			new Color(0.6f, 0.6f, 1f),
			new Color(0.5f, 0.5f, 0.5f),
			new Color(0.5f, 0.3f, 0f),
			new Color(1f, 0.6f, 0.18f),
			new Color(1f, 0f, 0f)
		};

		// Token: 0x04005805 RID: 22533
		private const float ElevationMax = 5000f;

		// Token: 0x04005806 RID: 22534
		private static Material[] matsElevation;

		// Token: 0x04005807 RID: 22535
		private static readonly Color[] ElevationSpectrum = new Color[]
		{
			new Color(0.224f, 0.18f, 0.15f),
			new Color(0.447f, 0.369f, 0.298f),
			new Color(0.6f, 0.6f, 0.6f),
			new Color(1f, 1f, 1f)
		};

		// Token: 0x04005808 RID: 22536
		private const float RainfallMax = 5000f;

		// Token: 0x04005809 RID: 22537
		private static Material[] matsRainfall;

		// Token: 0x0400580A RID: 22538
		private static readonly Color[] RainfallSpectrum = new Color[]
		{
			new Color(0.9f, 0.9f, 0.9f),
			GenColor.FromBytes(190, 190, 190, 255),
			new Color(0.58f, 0.58f, 0.58f),
			GenColor.FromBytes(196, 112, 110, 255),
			GenColor.FromBytes(200, 179, 150, 255),
			GenColor.FromBytes(255, 199, 117, 255),
			GenColor.FromBytes(255, 255, 84, 255),
			GenColor.FromBytes(145, 255, 253, 255),
			GenColor.FromBytes(0, 255, 0, 255),
			GenColor.FromBytes(63, 198, 55, 255),
			GenColor.FromBytes(13, 150, 5, 255),
			GenColor.FromBytes(5, 112, 94, 255)
		};
	}
}
