using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020002B9 RID: 697
	public static class RockNoises
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x000C3814 File Offset: 0x000C1A14
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

		// Token: 0x060011BB RID: 4539 RVA: 0x00012DAF File Offset: 0x00010FAF
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04000E5A RID: 3674
		public static List<RockNoises.RockNoise> rockNoises;

		// Token: 0x04000E5B RID: 3675
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x020002BA RID: 698
		public class RockNoise
		{
			// Token: 0x04000E5C RID: 3676
			public ThingDef rockDef;

			// Token: 0x04000E5D RID: 3677
			public ModuleBase noise;
		}
	}
}
