using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020001EF RID: 495
	public static class RockNoises
	{
		// Token: 0x06000DF1 RID: 3569 RVA: 0x0004EB3C File Offset: 0x0004CD3C
		public static void Init(Map map)
		{
			RockNoises.rockNoises = new List<RockNoises.RockNoise>();
			foreach (ThingDef rockDef in Find.World.NaturalRockTypesIn(map.Tile))
			{
				RockNoises.RockNoise rockNoise = new RockNoises.RockNoise();
				rockNoise.rockDef = rockDef;
				rockNoise.noise = new Perlin(0.004999999888241291, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.Medium);
				RockNoises.rockNoises.Add(rockNoise);
				NoiseDebugUI.StoreNoiseRender(rockNoise.noise, rockNoise.rockDef + " score", map.Size.ToIntVec2);
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0004EC10 File Offset: 0x0004CE10
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04000B68 RID: 2920
		public static List<RockNoises.RockNoise> rockNoises;

		// Token: 0x04000B69 RID: 2921
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x0200197A RID: 6522
		public class RockNoise
		{
			// Token: 0x040061BF RID: 25023
			public ThingDef rockDef;

			// Token: 0x040061C0 RID: 25024
			public ModuleBase noise;
		}
	}
}
