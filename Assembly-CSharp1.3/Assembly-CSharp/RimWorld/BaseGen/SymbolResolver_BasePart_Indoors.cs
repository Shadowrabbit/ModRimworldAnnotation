using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A3 RID: 5539
	public class SymbolResolver_BasePart_Indoors : SymbolResolver
	{
		// Token: 0x060082BF RID: 33471 RVA: 0x002E7874 File Offset: 0x002E5A74
		public override void Resolve(ResolveParams rp)
		{
			int? minLengthAfterSplit = null;
			bool flag;
			if (BaseGen.globalSettings.basePart_worshippedTerminalsResolved < BaseGen.globalSettings.requiredWorshippedTerminalRooms)
			{
				minLengthAfterSplit = new int?(7);
				flag = (rp.rect.Width >= 14 && rp.rect.Height >= 14);
			}
			else
			{
				flag = (rp.rect.Width > 13 || rp.rect.Height > 13 || ((rp.rect.Width >= 9 || rp.rect.Height >= 9) && Rand.Chance(0.3f)));
			}
			if (flag)
			{
				ResolveParams resolveParams = rp;
				resolveParams.minLengthAfterSplit = minLengthAfterSplit;
				BaseGen.symbolStack.Push("basePart_indoors_division", resolveParams, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_indoors_leaf", rp, null);
		}
	}
}
