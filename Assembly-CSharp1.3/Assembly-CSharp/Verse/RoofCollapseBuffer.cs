using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000210 RID: 528
	public class RoofCollapseBuffer
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x00055B1C File Offset: 0x00053D1C
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00055B24 File Offset: 0x00053D24
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00055B32 File Offset: 0x00053D32
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00055B4E File Offset: 0x00053D4E
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04000C0B RID: 3083
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
