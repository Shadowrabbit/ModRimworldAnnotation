using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000CAC RID: 3244
	public static class BeachMaker
	{
		// Token: 0x06004BA5 RID: 19365 RVA: 0x00192DFC File Offset: 0x00190FFC
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

		// Token: 0x06004BA6 RID: 19366 RVA: 0x00192FB1 File Offset: 0x001911B1
		public static void Cleanup()
		{
			BeachMaker.beachNoise = null;
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x00192FBC File Offset: 0x001911BC
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

		// Token: 0x04002DCE RID: 11726
		private static ModuleBase beachNoise;

		// Token: 0x04002DCF RID: 11727
		private const float PerlinFrequency = 0.03f;

		// Token: 0x04002DD0 RID: 11728
		private const float MaxForDeepWater = 0.1f;

		// Token: 0x04002DD1 RID: 11729
		private const float MaxForShallowWater = 0.45f;

		// Token: 0x04002DD2 RID: 11730
		private const float MaxForSand = 1f;

		// Token: 0x04002DD3 RID: 11731
		private static readonly FloatRange CoastWidthRange = new FloatRange(20f, 60f);
	}
}
