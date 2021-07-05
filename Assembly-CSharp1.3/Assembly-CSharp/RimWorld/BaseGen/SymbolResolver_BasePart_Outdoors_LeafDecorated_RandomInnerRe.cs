using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A7 RID: 5543
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x060082D2 RID: 33490 RVA: 0x002E8590 File Offset: 0x002E6790
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x060082D3 RID: 33491 RVA: 0x002E85E8 File Offset: 0x002E67E8
		public override void Resolve(ResolveParams rp)
		{
			int num = Rand.RangeInclusive(5, rp.rect.Width - 1);
			int num2 = Rand.RangeInclusive(5, rp.rect.Height - 1);
			int num3 = Rand.RangeInclusive(0, rp.rect.Width - num);
			int num4 = Rand.RangeInclusive(0, rp.rect.Height - num2);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX + num3, rp.rect.minZ + num4, num, num2);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams, null);
		}

		// Token: 0x040051E6 RID: 20966
		private const int MinLength = 5;

		// Token: 0x040051E7 RID: 20967
		private const int MaxRectSize = 15;
	}
}
