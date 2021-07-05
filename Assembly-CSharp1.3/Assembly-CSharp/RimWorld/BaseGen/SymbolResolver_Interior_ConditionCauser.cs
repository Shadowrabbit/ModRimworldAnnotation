using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015FC RID: 5628
	public class SymbolResolver_Interior_ConditionCauser : SymbolResolver
	{
		// Token: 0x060083E8 RID: 33768 RVA: 0x002F381C File Offset: 0x002F1A1C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingToSpawn = rp.conditionCauser;
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
