using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EB2 RID: 7858
	public abstract class SymbolResolver
	{
		// Token: 0x0600A8B5 RID: 43189 RVA: 0x0006F02E File Offset: 0x0006D22E
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x0600A8B6 RID: 43190
		public abstract void Resolve(ResolveParams rp);

		// Token: 0x04007268 RID: 29288
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04007269 RID: 29289
		public float selectionWeight = 1f;
	}
}
