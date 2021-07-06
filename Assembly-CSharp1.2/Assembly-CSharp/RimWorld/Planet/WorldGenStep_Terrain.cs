﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x020020AE RID: 8366
	public class WorldGenStep_Terrain : WorldGenStep
	{
		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x0600B12F RID: 45359 RVA: 0x000731D5 File Offset: 0x000713D5
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x0600B130 RID: 45360 RVA: 0x0000CE6C File Offset: 0x0000B06C
		private static float FreqMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600B131 RID: 45361 RVA: 0x000731DC File Offset: 0x000713DC
		public override void GenerateFresh(string seed)
		{
			this.GenerateGridIntoWorld();
		}

		// Token: 0x0600B132 RID: 45362 RVA: 0x000731E4 File Offset: 0x000713E4
		public override void GenerateFromScribe(string seed)
		{
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x0600B133 RID: 45363 RVA: 0x003368B8 File Offset: 0x00334AB8
		private void GenerateGridIntoWorld()
		{
			Find.World.grid = new WorldGrid();
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
			this.SetupElevationNoise();
			this.SetupTemperatureOffsetNoise();
			this.SetupRainfallNoise();
			this.SetupHillinessNoise();
			this.SetupSwampinessNoise();
			List<Tile> tiles = Find.WorldGrid.tiles;
			tiles.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile item = this.GenerateTileFor(i);
				tiles.Add(item);
			}
		}

		// Token: 0x0600B134 RID: 45364 RVA: 0x00336940 File Offset: 0x00334B40
		private void SetupElevationNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase lhs = new Perlin((double)(0.035f * freqMultiplier), 2.0, 0.4000000059604645, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new RidgedMultifractal((double)(0.012f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new Perlin((double)(0.12f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase3 = new Perlin((double)(0.01f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			float num;
			if (Find.World.PlanetCoverage < 0.55f)
			{
				ModuleBase moduleBase4 = new DistanceFromPlanetViewCenter(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, true);
				moduleBase4 = new ScaleBias(2.0, -1.0, moduleBase4);
				moduleBase3 = new Blend(moduleBase3, moduleBase4, new Const(0.4000000059604645));
				num = Rand.Range(-0.4f, -0.35f);
			}
			else
			{
				num = Rand.Range(0.15f, 0.25f);
			}
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevContinents");
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			moduleBase = new Multiply(moduleBase, moduleBase2);
			float num2 = Rand.Range(0.4f, 0.6f);
			this.noiseElevation = new Blend(lhs, moduleBase, new Const((double)num2));
			this.noiseElevation = new Blend(this.noiseElevation, moduleBase3, new Const((double)num));
			if (Find.World.PlanetCoverage < 0.9999f)
			{
				this.noiseElevation = new ConvertToIsland(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, this.noiseElevation);
			}
			this.noiseElevation = new ScaleBias(0.5, 0.5, this.noiseElevation);
			this.noiseElevation = new Power(this.noiseElevation, new Const(3.0));
			NoiseDebugUI.StorePlanetNoise(this.noiseElevation, "noiseElevation");
			this.noiseElevation = new ScaleBias((double)WorldGenStep_Terrain.ElevationRange.Span, (double)WorldGenStep_Terrain.ElevationRange.min, this.noiseElevation);
		}

		// Token: 0x0600B135 RID: 45365 RVA: 0x00336BA8 File Offset: 0x00334DA8
		private void SetupTemperatureOffsetNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseTemperatureOffset = new Perlin((double)(0.018f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseTemperatureOffset = new Multiply(this.noiseTemperatureOffset, new Const(4.0));
		}

		// Token: 0x0600B136 RID: 45366 RVA: 0x00336C0C File Offset: 0x00334E0C
		private void SetupRainfallNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.015f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			NoiseDebugUI.StorePlanetNoise(moduleBase, "basePerlin");
			ModuleBase moduleBase2 = new AbsLatitudeCurve(new SimpleCurve
			{
				{
					0f,
					1.12f,
					true
				},
				{
					25f,
					0.94f,
					true
				},
				{
					45f,
					0.7f,
					true
				},
				{
					70f,
					0.3f,
					true
				},
				{
					80f,
					0.05f,
					true
				},
				{
					90f,
					0.05f,
					true
				}
			}, 100f);
			NoiseDebugUI.StorePlanetNoise(moduleBase2, "latCurve");
			this.noiseRainfall = new Multiply(moduleBase, moduleBase2);
			float num = 0.00022222222f;
			float num2 = -500f * num;
			ModuleBase moduleBase3 = new ScaleBias((double)num, (double)num2, this.noiseElevation);
			moduleBase3 = new ScaleBias(-1.0, 1.0, moduleBase3);
			moduleBase3 = new Clamp(0.0, 1.0, moduleBase3);
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevationRainfallEffect");
			this.noiseRainfall = new Multiply(this.noiseRainfall, moduleBase3);
			Func<double, double> processor = delegate(double val)
			{
				if (val < 0.0)
				{
					val = 0.0;
				}
				if (val < 0.12)
				{
					val = (val + 0.12) / 2.0;
					if (val < 0.03)
					{
						val = (val + 0.03) / 2.0;
					}
				}
				return val;
			};
			this.noiseRainfall = new Arbitrary(this.noiseRainfall, processor);
			this.noiseRainfall = new Power(this.noiseRainfall, new Const(1.5));
			this.noiseRainfall = new Clamp(0.0, 999.0, this.noiseRainfall);
			NoiseDebugUI.StorePlanetNoise(this.noiseRainfall, "noiseRainfall before mm");
			this.noiseRainfall = new ScaleBias(4000.0, 0.0, this.noiseRainfall);
			SimpleCurve rainfallCurve = Find.World.info.overallRainfall.GetRainfallCurve();
			if (rainfallCurve != null)
			{
				this.noiseRainfall = new CurveSimple(this.noiseRainfall, rainfallCurve);
			}
		}

		// Token: 0x0600B137 RID: 45367 RVA: 0x00336E4C File Offset: 0x0033504C
		private void SetupHillinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseMountainLines = new Perlin((double)(0.025f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new Perlin((double)(0.06f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseMountainLines = new Abs(this.noiseMountainLines);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			moduleBase = new Filter(moduleBase, -0.3f, 1f);
			this.noiseMountainLines = new Multiply(this.noiseMountainLines, moduleBase);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			NoiseDebugUI.StorePlanetNoise(this.noiseMountainLines, "noiseMountainLines");
			this.noiseHillsPatchesMacro = new Perlin((double)(0.032f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			this.noiseHillsPatchesMicro = new Perlin((double)(0.19f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
		}

		// Token: 0x0600B138 RID: 45368 RVA: 0x00336F88 File Offset: 0x00335188
		private void SetupSwampinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.09f * freqMultiplier), 2.0, 0.4000000059604645, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new RidgedMultifractal((double)(0.025f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			this.noiseSwampiness = new Multiply(moduleBase, moduleBase2);
			InverseLerp rhs = new InverseLerp(this.noiseElevation, WorldGenStep_Terrain.SwampinessMaxElevation.max, WorldGenStep_Terrain.SwampinessMaxElevation.min);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs);
			InverseLerp rhs2 = new InverseLerp(this.noiseRainfall, WorldGenStep_Terrain.SwampinessMinRainfall.min, WorldGenStep_Terrain.SwampinessMinRainfall.max);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs2);
			NoiseDebugUI.StorePlanetNoise(this.noiseSwampiness, "noiseSwampiness");
		}

		// Token: 0x0600B139 RID: 45369 RVA: 0x003370A4 File Offset: 0x003352A4
		private Tile GenerateTileFor(int tileID)
		{
			Tile tile = new Tile();
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			tile.elevation = this.noiseElevation.GetValue(tileCenter);
			float value = this.noiseMountainLines.GetValue(tileCenter);
			if (value > 0.235f || tile.elevation <= 0f)
			{
				if (tile.elevation > 0f && this.noiseHillsPatchesMicro.GetValue(tileCenter) > 0.46f && this.noiseHillsPatchesMacro.GetValue(tileCenter) > -0.3f)
				{
					if (Rand.Bool)
					{
						tile.hilliness = Hilliness.SmallHills;
					}
					else
					{
						tile.hilliness = Hilliness.LargeHills;
					}
				}
				else
				{
					tile.hilliness = Hilliness.Flat;
				}
			}
			else if (value > 0.12f)
			{
				switch (Rand.Range(0, 4))
				{
				case 0:
					tile.hilliness = Hilliness.Flat;
					break;
				case 1:
					tile.hilliness = Hilliness.SmallHills;
					break;
				case 2:
					tile.hilliness = Hilliness.LargeHills;
					break;
				case 3:
					tile.hilliness = Hilliness.Mountainous;
					break;
				}
			}
			else if (value > 0.0363f)
			{
				tile.hilliness = Hilliness.Mountainous;
			}
			else
			{
				tile.hilliness = Hilliness.Impassable;
			}
			float num = WorldGenStep_Terrain.BaseTemperatureAtLatitude(Find.WorldGrid.LongLatOf(tileID).y);
			num -= WorldGenStep_Terrain.TemperatureReductionAtElevation(tile.elevation);
			num += this.noiseTemperatureOffset.GetValue(tileCenter);
			SimpleCurve temperatureCurve = Find.World.info.overallTemperature.GetTemperatureCurve();
			if (temperatureCurve != null)
			{
				num = temperatureCurve.Evaluate(num);
			}
			tile.temperature = num;
			tile.rainfall = this.noiseRainfall.GetValue(tileCenter);
			if (float.IsNaN(tile.rainfall))
			{
				Log.ErrorOnce(this.noiseRainfall.GetValue(tileCenter) + " rain bad at " + tileID, 694822, false);
			}
			if (tile.hilliness == Hilliness.Flat || tile.hilliness == Hilliness.SmallHills)
			{
				tile.swampiness = this.noiseSwampiness.GetValue(tileCenter);
			}
			tile.biome = this.BiomeFrom(tile, tileID);
			return tile;
		}

		// Token: 0x0600B13A RID: 45370 RVA: 0x00337290 File Offset: 0x00335490
		private BiomeDef BiomeFrom(Tile ws, int tileID)
		{
			List<BiomeDef> allDefsListForReading = DefDatabase<BiomeDef>.AllDefsListForReading;
			BiomeDef biomeDef = null;
			float num = 0f;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				BiomeDef biomeDef2 = allDefsListForReading[i];
				if (biomeDef2.implemented)
				{
					float score = biomeDef2.Worker.GetScore(ws, tileID);
					if (score > num || biomeDef == null)
					{
						biomeDef = biomeDef2;
						num = score;
					}
				}
			}
			return biomeDef;
		}

		// Token: 0x0600B13B RID: 45371 RVA: 0x003372F0 File Offset: 0x003354F0
		private static float FertilityFactorFromTemperature(float temp)
		{
			if (temp < -15f)
			{
				return 0f;
			}
			if (temp < 30f)
			{
				return Mathf.InverseLerp(-15f, 30f, temp);
			}
			if (temp < 50f)
			{
				return Mathf.InverseLerp(50f, 30f, temp);
			}
			return 0f;
		}

		// Token: 0x0600B13C RID: 45372 RVA: 0x00337344 File Offset: 0x00335544
		private static float BaseTemperatureAtLatitude(float lat)
		{
			float x = Mathf.Abs(lat) / 90f;
			return WorldGenStep_Terrain.AvgTempByLatitudeCurve.Evaluate(x);
		}

		// Token: 0x0600B13D RID: 45373 RVA: 0x0033736C File Offset: 0x0033556C
		private static float TemperatureReductionAtElevation(float elev)
		{
			if (elev < 250f)
			{
				return 0f;
			}
			float t = (elev - 250f) / 4750f;
			return Mathf.Lerp(0f, 40f, t);
		}

		// Token: 0x04007A03 RID: 31235
		[Unsaved(false)]
		private ModuleBase noiseElevation;

		// Token: 0x04007A04 RID: 31236
		[Unsaved(false)]
		private ModuleBase noiseTemperatureOffset;

		// Token: 0x04007A05 RID: 31237
		[Unsaved(false)]
		private ModuleBase noiseRainfall;

		// Token: 0x04007A06 RID: 31238
		[Unsaved(false)]
		private ModuleBase noiseSwampiness;

		// Token: 0x04007A07 RID: 31239
		[Unsaved(false)]
		private ModuleBase noiseMountainLines;

		// Token: 0x04007A08 RID: 31240
		[Unsaved(false)]
		private ModuleBase noiseHillsPatchesMicro;

		// Token: 0x04007A09 RID: 31241
		[Unsaved(false)]
		private ModuleBase noiseHillsPatchesMacro;

		// Token: 0x04007A0A RID: 31242
		private const float ElevationFrequencyMicro = 0.035f;

		// Token: 0x04007A0B RID: 31243
		private const float ElevationFrequencyMacro = 0.012f;

		// Token: 0x04007A0C RID: 31244
		private const float ElevationMacroFactorFrequency = 0.12f;

		// Token: 0x04007A0D RID: 31245
		private const float ElevationContinentsFrequency = 0.01f;

		// Token: 0x04007A0E RID: 31246
		private const float MountainLinesFrequency = 0.025f;

		// Token: 0x04007A0F RID: 31247
		private const float MountainLinesHolesFrequency = 0.06f;

		// Token: 0x04007A10 RID: 31248
		private const float HillsPatchesFrequencyMicro = 0.19f;

		// Token: 0x04007A11 RID: 31249
		private const float HillsPatchesFrequencyMacro = 0.032f;

		// Token: 0x04007A12 RID: 31250
		private const float SwampinessFrequencyMacro = 0.025f;

		// Token: 0x04007A13 RID: 31251
		private const float SwampinessFrequencyMicro = 0.09f;

		// Token: 0x04007A14 RID: 31252
		private static readonly FloatRange SwampinessMaxElevation = new FloatRange(650f, 750f);

		// Token: 0x04007A15 RID: 31253
		private static readonly FloatRange SwampinessMinRainfall = new FloatRange(725f, 900f);

		// Token: 0x04007A16 RID: 31254
		private static readonly FloatRange ElevationRange = new FloatRange(-500f, 5000f);

		// Token: 0x04007A17 RID: 31255
		private const float TemperatureOffsetFrequency = 0.018f;

		// Token: 0x04007A18 RID: 31256
		private const float TemperatureOffsetFactor = 4f;

		// Token: 0x04007A19 RID: 31257
		private static readonly SimpleCurve AvgTempByLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(0.1f, 29f),
				true
			},
			{
				new CurvePoint(0.5f, 7f),
				true
			},
			{
				new CurvePoint(1f, -37f),
				true
			}
		};

		// Token: 0x04007A1A RID: 31258
		private const float ElevationTempReductionStartAlt = 250f;

		// Token: 0x04007A1B RID: 31259
		private const float ElevationTempReductionEndAlt = 5000f;

		// Token: 0x04007A1C RID: 31260
		private const float MaxElevationTempReduction = 40f;

		// Token: 0x04007A1D RID: 31261
		private const float RainfallOffsetFrequency = 0.013f;

		// Token: 0x04007A1E RID: 31262
		private const float RainfallPower = 1.5f;

		// Token: 0x04007A1F RID: 31263
		private const float RainfallFactor = 4000f;

		// Token: 0x04007A20 RID: 31264
		private const float RainfallStartFallAltitude = 500f;

		// Token: 0x04007A21 RID: 31265
		private const float RainfallFinishFallAltitude = 5000f;

		// Token: 0x04007A22 RID: 31266
		private const float FertilityTempMinimum = -15f;

		// Token: 0x04007A23 RID: 31267
		private const float FertilityTempOptimal = 30f;

		// Token: 0x04007A24 RID: 31268
		private const float FertilityTempMaximum = 50f;
	}
}
