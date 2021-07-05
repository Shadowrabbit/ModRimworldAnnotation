using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015F6 RID: 5622
	public class SymbolResolver_WorkSite : SymbolResolver
	{
		// Token: 0x060083DB RID: 33755 RVA: 0x002F3342 File Offset: 0x002F1542
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp, null);
		}
	}
}
