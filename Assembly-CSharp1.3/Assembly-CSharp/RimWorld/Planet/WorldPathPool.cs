using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200172F RID: 5935
	public class WorldPathPool
	{
		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x060088E8 RID: 35048 RVA: 0x003135FA File Offset: 0x003117FA
		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		// Token: 0x060088EA RID: 35050 RVA: 0x00313610 File Offset: 0x00311810
		public WorldPath GetEmptyWorldPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > Find.WorldObjects.CaravansCount + 2 + (Find.WorldObjects.RoutePlannerWaypointsCount - 1))
			{
				Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788);
				this.paths.Clear();
			}
			WorldPath worldPath = new WorldPath();
			this.paths.Add(worldPath);
			worldPath.inUse = true;
			return worldPath;
		}

		// Token: 0x040056F4 RID: 22260
		private List<WorldPath> paths = new List<WorldPath>(64);

		// Token: 0x040056F5 RID: 22261
		private static readonly WorldPath notFoundPathInt = WorldPath.NewNotFound();
	}
}
