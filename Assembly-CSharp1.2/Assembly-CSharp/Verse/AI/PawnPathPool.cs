using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A5A RID: 2650
	public class PawnPathPool
	{
		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x0002F5EA File Offset: 0x0002D7EA
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0002F5F1 File Offset: 0x0002D7F1
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0017BAE8 File Offset: 0x00179CE8
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
				Log.ErrorOnce("PawnPathPool leak: more paths than spawned pawns. Force-recovering.", 664788, false);
				this.paths.Clear();
			}
			PawnPath pawnPath = new PawnPath();
			this.paths.Add(pawnPath);
			pawnPath.inUse = true;
			return pawnPath;
		}

		// Token: 0x04002B69 RID: 11113
		private Map map;

		// Token: 0x04002B6A RID: 11114
		private List<PawnPath> paths = new List<PawnPath>(64);

		// Token: 0x04002B6B RID: 11115
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();
	}
}
