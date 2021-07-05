using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A8 RID: 5544
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x060082D5 RID: 33493 RVA: 0x002E8688 File Offset: 0x002E6888
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings || rp.rect.Width < 9 || rp.rect.Height < 9) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x060082D6 RID: 33494 RVA: 0x002E8720 File Offset: 0x002E6920
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
