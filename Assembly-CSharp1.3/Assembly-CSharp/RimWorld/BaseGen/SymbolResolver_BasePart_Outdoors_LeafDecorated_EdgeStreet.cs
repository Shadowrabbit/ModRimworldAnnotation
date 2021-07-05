using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A6 RID: 5542
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_EdgeStreet : SymbolResolver
	{
		// Token: 0x060082D0 RID: 33488 RVA: 0x002E852C File Offset: 0x002E672C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.floorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			BaseGen.symbolStack.Push("edgeStreet", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.rect = rp.rect.ContractedBy(1);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams2, null);
		}
	}
}
