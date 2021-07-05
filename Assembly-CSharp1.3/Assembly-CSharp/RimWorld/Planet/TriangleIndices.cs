using System;

namespace RimWorld.Planet
{
	// Token: 0x02001782 RID: 6018
	public struct TriangleIndices
	{
		// Token: 0x06008AD9 RID: 35545 RVA: 0x0031D998 File Offset: 0x0031BB98
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x06008ADA RID: 35546 RVA: 0x0031D9B0 File Offset: 0x0031BBB0
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x06008ADB RID: 35547 RVA: 0x0031DA5F File Offset: 0x0031BC5F
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

		// Token: 0x04005875 RID: 22645
		public int v1;

		// Token: 0x04005876 RID: 22646
		public int v2;

		// Token: 0x04005877 RID: 22647
		public int v3;
	}
}
