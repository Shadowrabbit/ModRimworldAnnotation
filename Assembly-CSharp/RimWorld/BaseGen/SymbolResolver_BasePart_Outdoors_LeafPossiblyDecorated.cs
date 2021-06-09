using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E40 RID: 7744
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x0600A756 RID: 42838 RVA: 0x0030AF10 File Offset: 0x00309110
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.25f))
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leafDecorated", rp, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", rp, null);
		}
	}
}
