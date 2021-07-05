using System;

namespace Verse
{
	// Token: 0x0200046A RID: 1130
	public struct CellLine
	{
		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x000D9147 File Offset: 0x000D7347
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x000D914F File Offset: 0x000D734F
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000D9157 File Offset: 0x000D7357
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000D9167 File Offset: 0x000D7367
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000D9188 File Offset: 0x000D7388
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

		// Token: 0x06002249 RID: 8777 RVA: 0x000D91E0 File Offset: 0x000D73E0
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04001559 RID: 5465
		private float zIntercept;

		// Token: 0x0400155A RID: 5466
		private float slope;
	}
}
