using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E31 RID: 7729
	public class SymbolResolver_BasePart_Indoors_Leaf_ThroneRoom : SymbolResolver
	{
		// Token: 0x0600A720 RID: 42784 RVA: 0x00309A34 File Offset: 0x00307C34
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.faction != null && rp.faction == Faction.Empire && (BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms) && (BaseGen.globalSettings.basePart_throneRoomsResolved == 0 || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms);
		}

		// Token: 0x0600A721 RID: 42785 RVA: 0x0006E79E File Offset: 0x0006C99E
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("throneRoom", rp, null);
			BaseGen.globalSettings.basePart_throneRoomsResolved++;
		}
	}
}
