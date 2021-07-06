using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EA3 RID: 7843
	public class SymbolResolver_Interior_ConditionCauser : SymbolResolver
	{
		// Token: 0x0600A889 RID: 43145 RVA: 0x00311D9C File Offset: 0x0030FF9C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingToSpawn = rp.conditionCauser;
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
