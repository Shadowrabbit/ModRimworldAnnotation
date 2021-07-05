using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001597 RID: 5527
	public class GlobalSettings
	{
		// Token: 0x06008293 RID: 33427 RVA: 0x002E595C File Offset: 0x002E3B5C
		public void Clear()
		{
			this.map = null;
			this.minBuildings = 0;
			this.minBarracks = 0;
			this.requiredWorshippedTerminalRooms = 0;
			this.minEmptyNodes = 0;
			this.minLandingPads = 0;
			this.minThroneRooms = 0;
			this.maxFarms = -1;
			this.mainRect = CellRect.Empty;
			this.basePart_buildingsResolved = 0;
			this.basePart_emptyNodesResolved = 0;
			this.basePart_landingPadsResolved = 0;
			this.basePart_barracksResolved = 0;
			this.basePart_throneRoomsResolved = 0;
			this.basePart_batteriesCoverage = 0f;
			this.basePart_farmsCoverage = 0f;
			this.basePart_farmsCount = 0;
			this.basePart_powerPlantsCoverage = 0f;
			this.basePart_breweriesCoverage = 0f;
			this.basePart_worshippedTerminalsResolved = 0;
		}

		// Token: 0x06008294 RID: 33428 RVA: 0x002E5A09 File Offset: 0x002E3C09
		public void ClearResult()
		{
			this.landingPadsGenerated = 0;
		}

		// Token: 0x04005139 RID: 20793
		public Map map;

		// Token: 0x0400513A RID: 20794
		public int minBuildings;

		// Token: 0x0400513B RID: 20795
		public int minEmptyNodes;

		// Token: 0x0400513C RID: 20796
		public int minLandingPads;

		// Token: 0x0400513D RID: 20797
		public int minBarracks;

		// Token: 0x0400513E RID: 20798
		public int requiredWorshippedTerminalRooms;

		// Token: 0x0400513F RID: 20799
		public int minThroneRooms;

		// Token: 0x04005140 RID: 20800
		public int maxFarms = -1;

		// Token: 0x04005141 RID: 20801
		public CellRect mainRect;

		// Token: 0x04005142 RID: 20802
		public int basePart_buildingsResolved;

		// Token: 0x04005143 RID: 20803
		public int basePart_emptyNodesResolved;

		// Token: 0x04005144 RID: 20804
		public int basePart_landingPadsResolved;

		// Token: 0x04005145 RID: 20805
		public int basePart_barracksResolved;

		// Token: 0x04005146 RID: 20806
		public int basePart_throneRoomsResolved;

		// Token: 0x04005147 RID: 20807
		public int basePart_worshippedTerminalsResolved;

		// Token: 0x04005148 RID: 20808
		public float basePart_batteriesCoverage;

		// Token: 0x04005149 RID: 20809
		public float basePart_farmsCoverage;

		// Token: 0x0400514A RID: 20810
		public int basePart_farmsCount;

		// Token: 0x0400514B RID: 20811
		public float basePart_powerPlantsCoverage;

		// Token: 0x0400514C RID: 20812
		public float basePart_breweriesCoverage;

		// Token: 0x0400514D RID: 20813
		public int landingPadsGenerated;
	}
}
