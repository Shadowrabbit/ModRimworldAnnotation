using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E3E RID: 7742
	public class SymbolResolver_BasePart_Outdoors_Leaf_LandingPad : SymbolResolver
	{
		// Token: 0x0600A74D RID: 42829 RVA: 0x0030AB54 File Offset: 0x00308D54
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && (BaseGen.globalSettings.basePart_landingPadsResolved == 0 || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && rp.faction != null && rp.faction == Faction.Empire;
		}

		// Token: 0x0600A74E RID: 42830 RVA: 0x0030ABF0 File Offset: 0x00308DF0
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

		// Token: 0x040071B4 RID: 29108
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x040071B5 RID: 29109
		public const int Size = 9;
	}
}
