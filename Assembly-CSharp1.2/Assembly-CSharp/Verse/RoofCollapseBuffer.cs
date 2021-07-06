using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002F7 RID: 759
	public class RoofCollapseBuffer
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00013F9B File Offset: 0x0001219B
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00013FA3 File Offset: 0x000121A3
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00013FB1 File Offset: 0x000121B1
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00013FCD File Offset: 0x000121CD
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04000F7B RID: 3963
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
