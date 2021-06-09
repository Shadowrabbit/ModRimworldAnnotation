using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001266 RID: 4710
	public struct CellWithRadius : IEquatable<CellWithRadius>
	{
		// Token: 0x060066B0 RID: 26288 RVA: 0x000462E4 File Offset: 0x000444E4
		public CellWithRadius(IntVec3 c, float r)
		{
			this.cell = c;
			this.radius = r;
		}

		// Token: 0x060066B1 RID: 26289 RVA: 0x001F9E1C File Offset: 0x001F801C
		public bool Equals(CellWithRadius other)
		{
			return this.cell.Equals(other.cell) && this.radius.Equals(other.radius);
		}

		// Token: 0x060066B2 RID: 26290 RVA: 0x001F9E58 File Offset: 0x001F8058
		public override bool Equals(object obj)
		{
			if (obj is CellWithRadius)
			{
				CellWithRadius other = (CellWithRadius)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060066B3 RID: 26291 RVA: 0x001F9E80 File Offset: 0x001F8080
		public override int GetHashCode()
		{
			return this.cell.GetHashCode() * 397 ^ this.radius.GetHashCode();
		}

		// Token: 0x0400445A RID: 17498
		public readonly IntVec3 cell;

		// Token: 0x0400445B RID: 17499
		public readonly float radius;
	}
}
