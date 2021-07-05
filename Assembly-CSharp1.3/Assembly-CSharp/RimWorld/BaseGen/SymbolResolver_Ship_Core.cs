using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001605 RID: 5637
	public class SymbolResolver_Ship_Core : SymbolResolver
	{
		// Token: 0x06008401 RID: 33793 RVA: 0x002F41C0 File Offset: 0x002F23C0
		public override void Resolve(ResolveParams rp)
		{
			float value = Rand.Value;
			rp.faction = Faction.OfPlayer;
			ResolveParams resolveParams = rp;
			resolveParams.rect = resolveParams.rect.ContractedBy(2);
			resolveParams.hpPercentRange = new FloatRange?(new FloatRange(Mathf.Lerp(1.2f, 0.2f, value + Rand.Range(-0.2f, 0.2f)), 1.5f));
			BaseGen.symbolStack.Push("ship_pregen", resolveParams, null);
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.clearFillageOnly = new bool?(true);
			resolveParams2.clearRoof = new bool?(true);
			BaseGen.symbolStack.Push("clear", resolveParams2, null);
		}
	}
}
