using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E2C RID: 7724
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		// Token: 0x0600A711 RID: 42769 RVA: 0x0006E6EF File Offset: 0x0006C8EF
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms;
		}

		// Token: 0x0600A712 RID: 42770 RVA: 0x0006E715 File Offset: 0x0006C915
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp, null);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
