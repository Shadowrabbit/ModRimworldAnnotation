using System;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC0 RID: 2752
	public class SketchResolverDef : Def
	{
		// Token: 0x06004123 RID: 16675 RVA: 0x0015ECD5 File Offset: 0x0015CED5
		public void Resolve(ResolveParams parms)
		{
			this.resolver.Resolve(parms);
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x0015ECE3 File Offset: 0x0015CEE3
		public bool CanResolve(ResolveParams parms)
		{
			return this.resolver.CanResolve(parms);
		}

		// Token: 0x040026A9 RID: 9897
		public SketchResolver resolver;

		// Token: 0x040026AA RID: 9898
		public bool isRoot;
	}
}
