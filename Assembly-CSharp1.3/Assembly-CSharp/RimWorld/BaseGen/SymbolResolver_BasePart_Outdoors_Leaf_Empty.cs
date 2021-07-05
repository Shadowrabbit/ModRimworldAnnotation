using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A9 RID: 5545
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x060082D8 RID: 33496 RVA: 0x002E878C File Offset: 0x002E698C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9);
		}

		// Token: 0x060082D9 RID: 33497 RVA: 0x002E87F5 File Offset: 0x002E69F5
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
