using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001280 RID: 4736
	public class GenStep_CavesTerrain : GenStep
	{
		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x0600673D RID: 26429 RVA: 0x0004687B File Offset: 0x00044A7B
		public override int SeedPart
		{
			get
			{
				return 1921024373;
			}
		}

		// Token: 0x0600673E RID: 26430 RVA: 0x001FC24C File Offset: 0x001FA44C
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

		// Token: 0x040044A9 RID: 17577
		private const float WaterFrequency = 0.08f;

		// Token: 0x040044AA RID: 17578
		private const float GravelFrequency = 0.16f;

		// Token: 0x040044AB RID: 17579
		private const float WaterThreshold = 0.93f;

		// Token: 0x040044AC RID: 17580
		private const float GravelThreshold = 0.55f;
	}
}
