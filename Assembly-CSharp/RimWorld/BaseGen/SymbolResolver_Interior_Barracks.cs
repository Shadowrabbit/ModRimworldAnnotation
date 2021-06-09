using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EA0 RID: 7840
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x0600A882 RID: 43138 RVA: 0x0006EF2D File Offset: 0x0006D12D
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp, null);
		}
	}
}
