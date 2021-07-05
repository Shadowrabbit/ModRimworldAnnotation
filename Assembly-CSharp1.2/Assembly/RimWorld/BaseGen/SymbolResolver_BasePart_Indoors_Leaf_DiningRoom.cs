using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E2F RID: 7727
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		// Token: 0x0600A71A RID: 42778 RVA: 0x0006E73A File Offset: 0x0006C93A
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x0600A71B RID: 42779 RVA: 0x0006E778 File Offset: 0x0006C978
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp, null);
		}
	}
}
