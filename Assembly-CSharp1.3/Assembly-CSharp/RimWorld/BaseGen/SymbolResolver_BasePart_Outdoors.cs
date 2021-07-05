using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015AE RID: 5550
	public class SymbolResolver_BasePart_Outdoors : SymbolResolver
	{
		// Token: 0x060082E9 RID: 33513 RVA: 0x002E8DAC File Offset: 0x002E6FAC
		public override void Resolve(ResolveParams rp)
		{
			bool flag;
			if (BaseGen.globalSettings.basePart_worshippedTerminalsResolved < BaseGen.globalSettings.requiredWorshippedTerminalRooms)
			{
				flag = (rp.rect.Width >= 14 && rp.rect.Height >= 14);
			}
			else
			{
				flag = (rp.rect.Width > 23 || rp.rect.Height > 23 || ((rp.rect.Width >= 11 || rp.rect.Height >= 11) && Rand.Bool));
			}
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
