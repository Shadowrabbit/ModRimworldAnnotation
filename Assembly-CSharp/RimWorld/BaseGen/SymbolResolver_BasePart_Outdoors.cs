using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E41 RID: 7745
	public class SymbolResolver_BasePart_Outdoors : SymbolResolver
	{
		// Token: 0x0600A758 RID: 42840 RVA: 0x0030AF6C File Offset: 0x0030916C
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 23 || rp.rect.Height > 23 || ((rp.rect.Width >= 11 || rp.rect.Height >= 11) && Rand.Bool);
			ResolveParams resolveParams = rp;
			resolveParams.pathwayFloorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_outdoors_division", resolveParams, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leafPossiblyDecorated", resolveParams, null);
		}
	}
}
