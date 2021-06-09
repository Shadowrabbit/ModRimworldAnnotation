using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E47 RID: 7751
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x0600A769 RID: 42857 RVA: 0x0030B69C File Offset: 0x0030989C
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp, null);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
		}

		// Token: 0x040071BC RID: 29116
		private const float ExtraDoorChance = 0.25f;
	}
}
