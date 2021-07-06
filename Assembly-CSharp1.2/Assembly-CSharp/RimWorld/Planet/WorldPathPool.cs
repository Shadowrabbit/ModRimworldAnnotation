using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200200F RID: 8207
	public class WorldPathPool
	{
		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x0600ADD7 RID: 44503 RVA: 0x000712A3 File Offset: 0x0006F4A3
		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		// Token: 0x0600ADD9 RID: 44505 RVA: 0x00329B74 File Offset: 0x00327D74
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
				Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788, false);
				this.paths.Clear();
			}
			WorldPath worldPath = new WorldPath();
			this.paths.Add(worldPath);
			worldPath.inUse = true;
			return worldPath;
		}

		// Token: 0x04007766 RID: 30566
		private List<WorldPath> paths = new List<WorldPath>(64);

		// Token: 0x04007767 RID: 30567
		private static readonly WorldPath notFoundPathInt = WorldPath.NewNotFound();
	}
}
