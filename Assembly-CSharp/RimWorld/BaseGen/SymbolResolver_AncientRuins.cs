using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E72 RID: 7794
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x0600A7F3 RID: 42995 RVA: 0x0030E3D0 File Offset: 0x0030C5D0
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			resolveParams.chanceToSkipWallBlock = new float?(rp.chanceToSkipWallBlock ?? 0.1f);
			resolveParams.clearEdificeOnly = new bool?(rp.clearEdificeOnly ?? true);
			resolveParams.noRoof = new bool?(rp.noRoof ?? true);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams, null);
		}
	}
}
