using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000286 RID: 646
	public class RoadInfo : MapComponent
	{
		// Token: 0x060010E1 RID: 4321 RVA: 0x000126CA File Offset: 0x000108CA
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x000126DE File Offset: 0x000108DE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04000DB4 RID: 3508
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
