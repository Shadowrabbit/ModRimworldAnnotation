using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000602 RID: 1538
	public class PawnPathPool
	{
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x00107433 File Offset: 0x00105633
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x0010743A File Offset: 0x0010563A
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00107464 File Offset: 0x00105664
		public PawnPath GetEmptyPawnPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > this.map.mapPawns.AllPawnsSpawnedCount + 2)
			{
				Log.ErrorOnce("PawnPathPool leak: more paths than spawned pawns. Force-recovering.", 664788);
				this.paths.Clear();
			}
			PawnPath pawnPath = new PawnPath();
			this.paths.Add(pawnPath);
			pawnPath.inUse = true;
			return pawnPath;
		}

		// Token: 0x04001AE4 RID: 6884
		private Map map;

		// Token: 0x04001AE5 RID: 6885
		private List<PawnPath> paths = new List<PawnPath>(64);

		// Token: 0x04001AE6 RID: 6886
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();
	}
}
