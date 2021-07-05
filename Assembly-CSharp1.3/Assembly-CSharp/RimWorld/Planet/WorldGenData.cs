using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001783 RID: 6019
	public class WorldGenData : WorldComponent
	{
		// Token: 0x06008ADC RID: 35548 RVA: 0x0031DA87 File Offset: 0x0031BC87
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06008ADD RID: 35549 RVA: 0x0031DAA6 File Offset: 0x0031BCA6
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04005878 RID: 22648
		public List<int> roadNodes = new List<int>();

		// Token: 0x04005879 RID: 22649
		public List<int> ancientSites = new List<int>();
	}
}
