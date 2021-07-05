using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8A RID: 3210
	public class GenStep_ArchonexusResearchBuildings_SecondCycle : GenStep_ArchonexusResearchBuildings
	{
		// Token: 0x06004AE1 RID: 19169 RVA: 0x0018BC08 File Offset: 0x00189E08
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = CellRect.CenteredOn(loc, this.Size.x, this.Size.z);
			resolveParams.minorBuildingCount = new int?(4);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexusResearchBuildings", resolveParams, null);
			BaseGen.Generate();
		}
	}
}
