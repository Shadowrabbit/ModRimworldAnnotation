using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E30 RID: 7728
	public class SymbolResolver_BasePart_Indoors_Leaf_Storage : SymbolResolver
	{
		// Token: 0x0600A71D RID: 42781 RVA: 0x0006E73A File Offset: 0x0006C93A
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x0600A71E RID: 42782 RVA: 0x0006E78B File Offset: 0x0006C98B
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp, null);
		}
	}
}
