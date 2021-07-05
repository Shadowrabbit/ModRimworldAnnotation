﻿using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E3A RID: 7738
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x0600A741 RID: 42817 RVA: 0x0030A7B0 File Offset: 0x003089B0
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x0600A742 RID: 42818 RVA: 0x0030A808 File Offset: 0x00308A08
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

		// Token: 0x040071B1 RID: 29105
		private const int MinLength = 5;

		// Token: 0x040071B2 RID: 29106
		private const int MaxRectSize = 15;
	}
}
