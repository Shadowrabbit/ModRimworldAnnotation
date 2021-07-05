using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB5 RID: 2741
	public abstract class RoadDefGenStep
	{
		// Token: 0x06004100 RID: 16640
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);

		// Token: 0x04002665 RID: 9829
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x04002666 RID: 9830
		public float antialiasingMultiplier = 1f;

		// Token: 0x04002667 RID: 9831
		public int periodicSpacing;
	}
}
