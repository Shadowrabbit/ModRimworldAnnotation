using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004CD RID: 1229
	public class MapCellsInRandomOrder
	{
		// Token: 0x0600255E RID: 9566 RVA: 0x000E936D File Offset: 0x000E756D
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000E937C File Offset: 0x000E757C
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000E938A File Offset: 0x000E758A
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000E93A0 File Offset: 0x000E75A0
		private void CreateListIfShould()
		{
			if (this.randomizedCells != null)
			{
				return;
			}
			this.randomizedCells = new List<IntVec3>(this.map.Area);
			foreach (IntVec3 item in this.map.AllCells)
			{
				this.randomizedCells.Add(item);
			}
			Rand.PushState();
			Rand.Seed = (Find.World.info.Seed ^ this.map.Tile);
			this.randomizedCells.Shuffle<IntVec3>();
			Rand.PopState();
		}

		// Token: 0x04001740 RID: 5952
		private Map map;

		// Token: 0x04001741 RID: 5953
		private List<IntVec3> randomizedCells;
	}
}
