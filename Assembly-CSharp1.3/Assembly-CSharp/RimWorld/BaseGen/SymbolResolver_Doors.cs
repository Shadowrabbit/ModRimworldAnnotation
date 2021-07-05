using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B3 RID: 5555
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x060082F8 RID: 33528 RVA: 0x002E95B4 File Offset: 0x002E77B4
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp, null);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
		}

		// Token: 0x040051F0 RID: 20976
		private const float ExtraDoorChance = 0.25f;
	}
}
