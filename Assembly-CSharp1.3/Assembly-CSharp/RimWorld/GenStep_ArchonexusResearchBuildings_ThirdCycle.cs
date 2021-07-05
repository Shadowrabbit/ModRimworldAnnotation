using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8B RID: 3211
	public class GenStep_ArchonexusResearchBuildings_ThirdCycle : GenStep_ArchonexusResearchBuildings
	{
		// Token: 0x06004AE3 RID: 19171 RVA: 0x0018BC78 File Offset: 0x00189E78
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = CellRect.CenteredOn(loc, this.Size.x, this.Size.z);
			resolveParams.minorBuildingCount = new int?(8);
			resolveParams.minorBuildingRadialDistance = new int?(8);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexusResearchBuildings", resolveParams, null);
			BaseGen.Generate();
		}
	}
}
