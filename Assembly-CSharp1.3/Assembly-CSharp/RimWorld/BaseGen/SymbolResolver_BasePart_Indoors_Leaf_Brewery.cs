using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200159F RID: 5535
	public class SymbolResolver_BasePart_Indoors_Leaf_Brewery : SymbolResolver
	{
		// Token: 0x060082B2 RID: 33458 RVA: 0x002E752C File Offset: 0x002E572C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && BaseGen.globalSettings.basePart_breweriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.08f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Medieval);
		}

		// Token: 0x060082B3 RID: 33459 RVA: 0x002E75EC File Offset: 0x002E57EC
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("brewery", rp, null);
			BaseGen.globalSettings.basePart_breweriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x040051D7 RID: 20951
		private const float MaxCoverage = 0.08f;
	}
}
