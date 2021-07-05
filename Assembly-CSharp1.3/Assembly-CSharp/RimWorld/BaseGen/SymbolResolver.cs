using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001609 RID: 5641
	public abstract class SymbolResolver
	{
		// Token: 0x0600840A RID: 33802 RVA: 0x002F5091 File Offset: 0x002F3291
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x0600840B RID: 33803
		public abstract void Resolve(ResolveParams rp);

		// Token: 0x04005254 RID: 21076
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04005255 RID: 21077
		public float selectionWeight = 1f;
	}
}
