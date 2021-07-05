using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000C8E RID: 3214
	public class GenStep_CavesTerrain : GenStep
	{
		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x0018CD1F File Offset: 0x0018AF1F
		public override int SeedPart
		{
			get
			{
				return 1921024373;
			}
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0018CD28 File Offset: 0x0018AF28
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			Perlin perlin = new Perlin(0.07999999821186066, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			Perlin perlin2 = new Perlin(0.1599999964237213, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f && !intVec.GetTerrain(map).IsRiver)
				{
					float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					float num2 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					if (num > 0.93f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.WaterShallow);
					}
					else if (num2 > 0.55f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Gravel);
					}
				}
			}
		}

		// Token: 0x04002D78 RID: 11640
		private const float WaterFrequency = 0.08f;

		// Token: 0x04002D79 RID: 11641
		private const float GravelFrequency = 0.16f;

		// Token: 0x04002D7A RID: 11642
		private const float WaterThreshold = 0.93f;

		// Token: 0x04002D7B RID: 11643
		private const float GravelThreshold = 0.55f;
	}
}
