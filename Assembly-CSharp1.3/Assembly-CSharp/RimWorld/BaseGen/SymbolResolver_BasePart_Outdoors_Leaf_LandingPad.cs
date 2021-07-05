using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015AB RID: 5547
	public class SymbolResolver_BasePart_Outdoors_Leaf_LandingPad : SymbolResolver
	{
		// Token: 0x060082DE RID: 33502 RVA: 0x002E897C File Offset: 0x002E6B7C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && (BaseGen.globalSettings.basePart_landingPadsResolved == 0 || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && rp.faction != null && rp.faction == Faction.OfEmpire;
		}

		// Token: 0x060082DF RID: 33503 RVA: 0x002E8A18 File Offset: 0x002E6C18
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (!rp.rect.TryFindRandomInnerRect(new IntVec2(9, 9), out rect, null))
			{
				return;
			}
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			BaseGen.symbolStack.Push("landingPad", resolveParams, null);
			BaseGen.globalSettings.basePart_landingPadsResolved++;
		}

		// Token: 0x040051E9 RID: 20969
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x040051EA RID: 20970
		public const int Size = 9;
	}
}
