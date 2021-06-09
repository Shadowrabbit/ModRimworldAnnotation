using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E3B RID: 7739
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x0600A744 RID: 42820 RVA: 0x0030A8A8 File Offset: 0x00308AA8
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings || rp.rect.Width < 9 || rp.rect.Height < 9) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x0600A745 RID: 42821 RVA: 0x0030A940 File Offset: 0x00308B40
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
			resolveParams.floorDef = (rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, true));
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams, null);
			BaseGen.globalSettings.basePart_buildingsResolved++;
		}
	}
}
