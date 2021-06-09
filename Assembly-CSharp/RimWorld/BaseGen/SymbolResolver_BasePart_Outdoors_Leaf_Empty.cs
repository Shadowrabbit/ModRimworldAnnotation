using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E3C RID: 7740
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x0600A747 RID: 42823 RVA: 0x0030A9AC File Offset: 0x00308BAC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9);
		}

		// Token: 0x0600A748 RID: 42824 RVA: 0x0006E899 File Offset: 0x0006CA99
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
