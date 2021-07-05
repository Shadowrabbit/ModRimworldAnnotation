using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200159D RID: 5533
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		// Token: 0x060082AC RID: 33452 RVA: 0x002E73A0 File Offset: 0x002E55A0
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp));
		}

		// Token: 0x060082AD RID: 33453 RVA: 0x002E73F6 File Offset: 0x002E55F6
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp, null);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
