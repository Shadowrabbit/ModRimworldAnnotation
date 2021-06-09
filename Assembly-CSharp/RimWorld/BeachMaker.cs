using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012B4 RID: 4788
	public static class BeachMaker
	{
		// Token: 0x060067F0 RID: 26608 RVA: 0x00201178 File Offset: 0x001FF378
		public static void Init(Map map)
		{
			Rot4 a = Find.World.CoastDirectionAt(map.Tile);
			if (!a.IsValid)
			{
				BeachMaker.beachNoise = null;
				return;
			}
			ModuleBase moduleBase = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "BeachMaker base", map.Size.ToIntVec2);
			ModuleBase moduleBase2 = new DistFromAxis(BeachMaker.CoastWidthRange.RandomInRange);
			if (a == Rot4.North)
			{
				moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
				moduleBase2 = new Translate(0.0, 0.0, (double)(-(double)map.Size.z), moduleBase2);
			}
			else if (a == Rot4.East)
			{
				moduleBase2 = new Translate((double)(-(double)map.Size.x), 0.0, 0.0, moduleBase2);
			}
			else if (a == Rot4.South)
			{
				moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
			}
			moduleBase2 = new ScaleBias(1.0, -1.0, moduleBase2);
			moduleBase2 = new Clamp(-1.0, 2.5, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(moduleBase2, "BeachMaker axis bias");
			BeachMaker.beachNoise = new Add(moduleBase, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(BeachMaker.beachNoise, "beachNoise");
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x00046D00 File Offset: 0x00044F00
		public static void Cleanup()
		{
			BeachMaker.beachNoise = null;
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x00201330 File Offset: 0x001FF530
		public static TerrainDef BeachTerrainAt(IntVec3 loc, BiomeDef biome)
		{
			if (BeachMaker.beachNoise == null)
			{
				return null;
			}
			float value = BeachMaker.beachNoise.GetValue(loc);
			if (value < 0.1f)
			{
				return TerrainDefOf.WaterOceanDeep;
			}
			if (value < 0.45f)
			{
				return TerrainDefOf.WaterOceanShallow;
			}
			if (value >= 1f)
			{
				return null;
			}
			if (biome != BiomeDefOf.SeaIce)
			{
				return TerrainDefOf.Sand;
			}
			return TerrainDefOf.Ice;
		}

		// Token: 0x04004536 RID: 17718
		private static ModuleBase beachNoise;

		// Token: 0x04004537 RID: 17719
		private const float PerlinFrequency = 0.03f;

		// Token: 0x04004538 RID: 17720
		private const float MaxForDeepWater = 0.1f;

		// Token: 0x04004539 RID: 17721
		private const float MaxForShallowWater = 0.45f;

		// Token: 0x0400453A RID: 17722
		private const float MaxForSand = 1f;

		// Token: 0x0400453B RID: 17723
		private static readonly FloatRange CoastWidthRange = new FloatRange(20f, 60f);
	}
}
