using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002091 RID: 8337
	public class WorldGenData : WorldComponent
	{
		// Token: 0x0600B0C3 RID: 45251 RVA: 0x00072E7A File Offset: 0x0007107A
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x0600B0C4 RID: 45252 RVA: 0x00072E99 File Offset: 0x00071099
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x040079B8 RID: 31160
		public List<int> roadNodes = new List<int>();

		// Token: 0x040079B9 RID: 31161
		public List<int> ancientSites = new List<int>();
	}
}
