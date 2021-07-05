using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7B RID: 3195
	public struct CellWithRadius : IEquatable<CellWithRadius>
	{
		// Token: 0x06004A7D RID: 19069 RVA: 0x0018A280 File Offset: 0x00188480
		public CellWithRadius(IntVec3 c, float r)
		{
			this.cell = c;
			this.radius = r;
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x0018A290 File Offset: 0x00188490
		public bool Equals(CellWithRadius other)
		{
			return this.cell.Equals(other.cell) && this.radius.Equals(other.radius);
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0018A2CC File Offset: 0x001884CC
		public override bool Equals(object obj)
		{
			if (obj is CellWithRadius)
			{
				CellWithRadius other = (CellWithRadius)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0018A2F4 File Offset: 0x001884F4
		public override int GetHashCode()
		{
			return this.cell.GetHashCode() * 397 ^ this.radius.GetHashCode();
		}

		// Token: 0x04002D41 RID: 11585
		public readonly IntVec3 cell;

		// Token: 0x04002D42 RID: 11586
		public readonly float radius;
	}
}
