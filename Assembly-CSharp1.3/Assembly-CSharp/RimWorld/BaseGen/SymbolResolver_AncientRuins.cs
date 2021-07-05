using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015D2 RID: 5586
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x06008368 RID: 33640 RVA: 0x002EDA04 File Offset: 0x002EBC04
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
