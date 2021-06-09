using System;

namespace Verse
{
	// Token: 0x020007C4 RID: 1988
	public struct CellLine
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x000274C2 File Offset: 0x000256C2
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x000274CA File Offset: 0x000256CA
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x000274D2 File Offset: 0x000256D2
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000274E2 File Offset: 0x000256E2
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x0014BA5C File Offset: 0x00149C5C
		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num;
			if (a.x == b.x)
			{
				num = 100000000f;
			}
			else
			{
				num = (float)(b.z - a.z) / (float)(b.x - a.x);
			}
			return new CellLine((float)a.z - (float)a.x * num, num);
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x00027502 File Offset: 0x00025702
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04002281 RID: 8833
		private float zIntercept;

		// Token: 0x04002282 RID: 8834
		private float slope;
	}
}
