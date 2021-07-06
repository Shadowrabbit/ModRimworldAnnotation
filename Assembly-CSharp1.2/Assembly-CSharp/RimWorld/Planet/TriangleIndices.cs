using System;

namespace RimWorld.Planet
{
	// Token: 0x02002090 RID: 8336
	public struct TriangleIndices
	{
		// Token: 0x0600B0C0 RID: 45248 RVA: 0x00072E3B File Offset: 0x0007103B
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x0600B0C1 RID: 45249 RVA: 0x00335228 File Offset: 0x00333428
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x0600B0C2 RID: 45250 RVA: 0x00072E52 File Offset: 0x00071052
		public int GetNextOrderedVertex(int root)
		{
			if (this.v1 == root)
			{
				return this.v2;
			}
			if (this.v2 == root)
			{
				return this.v3;
			}
			return this.v1;
		}

		// Token: 0x040079B5 RID: 31157
		public int v1;

		// Token: 0x040079B6 RID: 31158
		public int v2;

		// Token: 0x040079B7 RID: 31159
		public int v3;
	}
}
