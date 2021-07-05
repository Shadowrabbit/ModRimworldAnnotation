using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015AA RID: 5546
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		// Token: 0x060082DB RID: 33499 RVA: 0x002E880C File Offset: 0x002E6A0C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9) && BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes && BaseGen.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.55f && (BaseGen.globalSettings.maxFarms <= -1 || BaseGen.globalSettings.basePart_farmsCount < BaseGen.globalSettings.maxFarms) && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x060082DC RID: 33500 RVA: 0x002E891C File Offset: 0x002E6B1C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("farm", rp, null);
			BaseGen.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
			BaseGen.globalSettings.basePart_farmsCount++;
		}

		// Token: 0x040051E8 RID: 20968
		private const float MaxCoverage = 0.55f;
	}
}
