using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E32 RID: 7730
	public class SymbolResolver_BasePart_Indoors : SymbolResolver
	{
		// Token: 0x0600A723 RID: 42787 RVA: 0x00309AB8 File Offset: 0x00307CB8
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 13 || rp.rect.Height > 13 || ((rp.rect.Width >= 9 || rp.rect.Height >= 9) && Rand.Chance(0.3f));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_indoors_division", rp, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_indoors_leaf", rp, null);
		}
	}
}
