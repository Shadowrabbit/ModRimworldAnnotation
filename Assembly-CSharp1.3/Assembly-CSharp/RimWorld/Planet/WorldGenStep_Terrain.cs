using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x0200178C RID: 6028
	public class WorldGenStep_Terrain : WorldGenStep
	{
		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x06008B0F RID: 35599 RVA: 0x0031ECA7 File Offset: 0x0031CEA7
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x06008B10 RID: 35600 RVA: 0x0001F15E File Offset: 0x0001D35E
		private static float FreqMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06008B11 RID: 35601 RVA: 0x0031ECAE File Offset: 0x0031CEAE
		public override void GenerateFresh(string seed)
		{
			this.GenerateGridIntoWorld();
		}

		// Token: 0x06008B12 RID: 35602 RVA: 0x0031ECB6 File Offset: 0x0031CEB6
		public override void GenerateFromScribe(string seed)
		{
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06008B13 RID: 35603 RVA: 0x0031ECCC File Offset: 0x0031CECC
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

		// Token: 0x06008B14 RID: 35604 RVA: 0x0031ED54 File Offset: 0x0031CF54
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

		// Token: 0x06008B15 RID: 35605 RVA: 0x0031EFBC File Offset: 0x0031D1BC
		private void SetupTemperatureOffsetNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseTemperatureOffset = new Perlin((double)(0.018f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseTemperatureOffset = new Multiply(this.noiseTemperatureOffset, new Const(4.0));
		}

		// Token: 0x06008B16 RID: 35606 RVA: 0x0031F020 File Offset: 0x0031D220
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

		// Token: 0x06008B17 RID: 35607 RVA: 0x0031F260 File Offset: 0x0031D460
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

		// Token: 0x06008B18 RID: 35608 RVA: 0x0031F39C File Offset: 0x0031D59C
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

		// Token: 0x06008B19 RID: 35609 RVA: 0x0031F4B8 File Offset: 0x0031D6B8
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
				Log.ErrorOnce(this.noiseRainfall.GetValue(tileCenter) + " rain bad at " + tileID, 694822);
			}
			if (tile.hilliness == Hilliness.Flat || tile.hilliness == Hilliness.SmallHills)
			{
				tile.swampiness = this.noiseSwampiness.GetValue(tileCenter);
			}
			tile.biome = this.BiomeFrom(tile, tileID);
			return tile;
		}

		// Token: 0x06008B1A RID: 35610 RVA: 0x0031F6A0 File Offset: 0x0031D8A0
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

		// Token: 0x06008B1B RID: 35611 RVA: 0x0031F700 File Offset: 0x0031D900
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

		// Token: 0x06008B1C RID: 35612 RVA: 0x0031F754 File Offset: 0x0031D954
		private static float BaseTemperatureAtLatitude(float lat)
		{
			float x = Mathf.Abs(lat) / 90f;
			return WorldGenStep_Terrain.AvgTempByLatitudeCurve.Evaluate(x);
		}

		// Token: 0x06008B1D RID: 35613 RVA: 0x0031F77C File Offset: 0x0031D97C
		private static float TemperatureReductionAtElevation(float elev)
		{
			if (elev < 250f)
			{
				return 0f;
			}
			float t = (elev - 250f) / 4750f;
			return Mathf.Lerp(0f, 40f, t);
		}

		// Token: 0x0400588C RID: 22668
		[Unsaved(false)]
		private ModuleBase noiseElevation;

		// Token: 0x0400588D RID: 22669
		[Unsaved(false)]
		private ModuleBase noiseTemperatureOffset;

		// Token: 0x0400588E RID: 22670
		[Unsaved(false)]
		private ModuleBase noiseRainfall;

		// Token: 0x0400588F RID: 22671
		[Unsaved(false)]
		private ModuleBase noiseSwampiness;

		// Token: 0x04005890 RID: 22672
		[Unsaved(false)]
		private ModuleBase noiseMountainLines;

		// Token: 0x04005891 RID: 22673
		[Unsaved(false)]
		private ModuleBase noiseHillsPatchesMicro;

		// Token: 0x04005892 RID: 22674
		[Unsaved(false)]
		private ModuleBase noiseHillsPatchesMacro;

		// Token: 0x04005893 RID: 22675
		private const float ElevationFrequencyMicro = 0.035f;

		// Token: 0x04005894 RID: 22676
		private const float ElevationFrequencyMacro = 0.012f;

		// Token: 0x04005895 RID: 22677
		private const float ElevationMacroFactorFrequency = 0.12f;

		// Token: 0x04005896 RID: 22678
		private const float ElevationContinentsFrequency = 0.01f;

		// Token: 0x04005897 RID: 22679
		private const float MountainLinesFrequency = 0.025f;

		// Token: 0x04005898 RID: 22680
		private const float MountainLinesHolesFrequency = 0.06f;

		// Token: 0x04005899 RID: 22681
		private const float HillsPatchesFrequencyMicro = 0.19f;

		// Token: 0x0400589A RID: 22682
		private const float HillsPatchesFrequencyMacro = 0.032f;

		// Token: 0x0400589B RID: 22683
		private const float SwampinessFrequencyMacro = 0.025f;

		// Token: 0x0400589C RID: 22684
		private const float SwampinessFrequencyMicro = 0.09f;

		// Token: 0x0400589D RID: 22685
		private static readonly FloatRange SwampinessMaxElevation = new FloatRange(650f, 750f);

		// Token: 0x0400589E RID: 22686
		private static readonly FloatRange SwampinessMinRainfall = new FloatRange(725f, 900f);

		// Token: 0x0400589F RID: 22687
		private static readonly FloatRange ElevationRange = new FloatRange(-500f, 5000f);

		// Token: 0x040058A0 RID: 22688
		private const float TemperatureOffsetFrequency = 0.018f;

		// Token: 0x040058A1 RID: 22689
		private const float TemperatureOffsetFactor = 4f;

		// Token: 0x040058A2 RID: 22690
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

		// Token: 0x040058A3 RID: 22691
		private const float ElevationTempReductionStartAlt = 250f;

		// Token: 0x040058A4 RID: 22692
		private const float ElevationTempReductionEndAlt = 5000f;

		// Token: 0x040058A5 RID: 22693
		private const float MaxElevationTempReduction = 40f;

		// Token: 0x040058A6 RID: 22694
		private const float RainfallOffsetFrequency = 0.013f;

		// Token: 0x040058A7 RID: 22695
		private const float RainfallPower = 1.5f;

		// Token: 0x040058A8 RID: 22696
		private const float RainfallFactor = 4000f;

		// Token: 0x040058A9 RID: 22697
		private const float RainfallStartFallAltitude = 500f;

		// Token: 0x040058AA RID: 22698
		private const float RainfallFinishFallAltitude = 5000f;

		// Token: 0x040058AB RID: 22699
		private const float FertilityTempMinimum = -15f;

		// Token: 0x040058AC RID: 22700
		private const float FertilityTempOptimal = 30f;

		// Token: 0x040058AD RID: 22701
		private const float FertilityTempMaximum = 50f;
	}
}
