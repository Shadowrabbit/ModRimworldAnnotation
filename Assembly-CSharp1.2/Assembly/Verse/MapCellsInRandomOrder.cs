using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000877 RID: 2167
	public class MapCellsInRandomOrder
	{
		// Token: 0x060035EA RID: 13802 RVA: 0x00029C4F File Offset: 0x00027E4F
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x00029C5E File Offset: 0x00027E5E
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x00029C6C File Offset: 0x00027E6C
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0015B024 File Offset: 0x00159224
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

		// Token: 0x04002580 RID: 9600
		private Map map;

		// Token: 0x04002581 RID: 9601
		private List<IntVec3> randomizedCells;
	}
}
