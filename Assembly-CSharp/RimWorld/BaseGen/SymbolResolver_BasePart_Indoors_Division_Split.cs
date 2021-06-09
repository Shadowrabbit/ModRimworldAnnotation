using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E2B RID: 7723
	public class SymbolResolver_BasePart_Indoors_Division_Split : SymbolResolver
	{
		// Token: 0x0600A70E RID: 42766 RVA: 0x0006E6B5 File Offset: 0x0006C8B5
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.rect.Width >= 9 || rp.rect.Height >= 9);
		}

		// Token: 0x0600A70F RID: 42767 RVA: 0x00309690 File Offset: 0x00307890
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width < 9 && rp.rect.Height < 9)
			{
				Log.Warning("Too small rect. params=" + rp, false);
				return;
			}
			if ((Rand.Bool && rp.rect.Height >= 9) || rp.rect.Width < 9)
			{
				int num = Rand.RangeInclusive(4, rp.rect.Height - 5);
				ResolveParams resolveParams = rp;
				resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num + 1);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams, null);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num, rp.rect.Width, rp.rect.Height - num);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams2, null);
				return;
			}
			int num2 = Rand.RangeInclusive(4, rp.rect.Width - 5);
			ResolveParams resolveParams3 = rp;
			resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num2 + 1, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams3, null);
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = new CellRect(rp.rect.minX + num2, rp.rect.minZ, rp.rect.Width - num2, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams4, null);
		}

		// Token: 0x04007195 RID: 29077
		private const int MinLengthAfterSplit = 5;

		// Token: 0x04007196 RID: 29078
		private const int MinWidthOrHeight = 9;
	}
}
