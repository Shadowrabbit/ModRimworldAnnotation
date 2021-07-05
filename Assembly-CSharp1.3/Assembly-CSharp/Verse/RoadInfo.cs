using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C6 RID: 454
	public class RoadInfo : MapComponent
	{
		// Token: 0x06000D35 RID: 3381 RVA: 0x00046C18 File Offset: 0x00044E18
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00046C2C File Offset: 0x00044E2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04000AD8 RID: 2776
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
