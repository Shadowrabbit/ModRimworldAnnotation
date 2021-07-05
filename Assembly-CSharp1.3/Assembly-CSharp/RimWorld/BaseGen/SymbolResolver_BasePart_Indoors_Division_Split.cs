using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200159C RID: 5532
	public class SymbolResolver_BasePart_Indoors_Division_Split : SymbolResolver
	{
		// Token: 0x060082A7 RID: 33447 RVA: 0x002E7140 File Offset: 0x002E5340
		private int ResolveMinLengthAfterSplit(ResolveParams rp)
		{
			int? minLengthAfterSplit = rp.minLengthAfterSplit;
			if (minLengthAfterSplit == null)
			{
				return 5;
			}
			return minLengthAfterSplit.GetValueOrDefault();
		}

		// Token: 0x060082A8 RID: 33448 RVA: 0x002E7166 File Offset: 0x002E5366
		private int ResolveMinWidthOrHeight(int minLengthAfterSplit)
		{
			return minLengthAfterSplit * 2 - 1;
		}

		// Token: 0x060082A9 RID: 33449 RVA: 0x002E7170 File Offset: 0x002E5370
		public override bool CanResolve(ResolveParams rp)
		{
			int num = this.ResolveMinWidthOrHeight(this.ResolveMinLengthAfterSplit(rp));
			return base.CanResolve(rp) && (rp.rect.Width >= num || rp.rect.Height >= num);
		}

		// Token: 0x060082AA RID: 33450 RVA: 0x002E71BC File Offset: 0x002E53BC
		public override void Resolve(ResolveParams rp)
		{
			int num = this.ResolveMinLengthAfterSplit(rp);
			int num2 = this.ResolveMinWidthOrHeight(num);
			if (rp.rect.Width < num2 && rp.rect.Height < num2)
			{
				Log.Warning("Too small rect. params=" + rp);
				return;
			}
			if ((Rand.Bool && rp.rect.Height >= num2) || rp.rect.Width < num2)
			{
				int num3 = Rand.RangeInclusive(num - 1, rp.rect.Height - num);
				ResolveParams resolveParams = rp;
				resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num3 + 1);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams, null);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num3, rp.rect.Width, rp.rect.Height - num3);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams2, null);
				return;
			}
			int num4 = Rand.RangeInclusive(num - 1, rp.rect.Width - num);
			ResolveParams resolveParams3 = rp;
			resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num4 + 1, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams3, null);
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = new CellRect(rp.rect.minX + num4, rp.rect.minZ, rp.rect.Width - num4, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams4, null);
		}

		// Token: 0x040051D5 RID: 20949
		private const int MinLengthAfterSplit = 5;
	}
}
