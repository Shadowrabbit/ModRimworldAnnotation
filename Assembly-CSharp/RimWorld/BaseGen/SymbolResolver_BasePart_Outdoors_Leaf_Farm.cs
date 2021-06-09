using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E3D RID: 7741
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		// Token: 0x0600A74A RID: 42826 RVA: 0x0030AA18 File Offset: 0x00308C18
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9) && BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes && BaseGen.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.55f && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x0600A74B RID: 42827 RVA: 0x0030AB04 File Offset: 0x00308D04
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("farm", rp, null);
			BaseGen.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x040071B3 RID: 29107
		private const float MaxCoverage = 0.55f;
	}
}
