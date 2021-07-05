using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015F9 RID: 5625
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x060083E1 RID: 33761 RVA: 0x002F3649 File Offset: 0x002F1849
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp, null);
		}
	}
}
