using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015AD RID: 5549
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x060082E7 RID: 33511 RVA: 0x002E8D50 File Offset: 0x002E6F50
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
