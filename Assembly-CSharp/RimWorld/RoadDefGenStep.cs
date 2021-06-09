using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD5 RID: 4053
	public abstract class RoadDefGenStep
	{
		// Token: 0x0600587E RID: 22654
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);

		// Token: 0x04003AA8 RID: 15016
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x04003AA9 RID: 15017
		public float antialiasingMultiplier = 1f;

		// Token: 0x04003AAA RID: 15018
		public int periodicSpacing;
	}
}
