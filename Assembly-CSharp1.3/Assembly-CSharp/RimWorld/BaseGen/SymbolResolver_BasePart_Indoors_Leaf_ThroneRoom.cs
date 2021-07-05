using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A1 RID: 5537
	public class SymbolResolver_BasePart_Indoors_Leaf_ThroneRoom : SymbolResolver
	{
		// Token: 0x060082B8 RID: 33464 RVA: 0x002E76C0 File Offset: 0x002E58C0
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.faction != null && rp.faction == Faction.OfEmpire && (rp.allowGeneratingThronerooms == null || rp.allowGeneratingThronerooms.Value) && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms) && (BaseGen.globalSettings.basePart_throneRoomsResolved == 0 || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms);
		}

		// Token: 0x060082B9 RID: 33465 RVA: 0x002E7785 File Offset: 0x002E5985
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("throneRoom", rp, null);
			BaseGen.globalSettings.basePart_throneRoomsResolved++;
		}
	}
}
