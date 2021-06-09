using System;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE1 RID: 4065
	public class SketchResolverDef : Def
	{
		// Token: 0x060058AF RID: 22703 RVA: 0x0003DA07 File Offset: 0x0003BC07
		public void Resolve(ResolveParams parms)
		{
			this.resolver.Resolve(parms);
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x0003DA15 File Offset: 0x0003BC15
		public bool CanResolve(ResolveParams parms)
		{
			return this.resolver.CanResolve(parms);
		}

		// Token: 0x04003AF1 RID: 15089
		public SketchResolver resolver;

		// Token: 0x04003AF2 RID: 15090
		public bool isRoot;
	}
}
