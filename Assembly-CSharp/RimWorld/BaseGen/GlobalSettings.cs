using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E29 RID: 7721
	public class GlobalSettings
	{
		// Token: 0x0600A706 RID: 42758 RVA: 0x00308B08 File Offset: 0x00306D08
		public void Clear()
		{
			this.map = null;
			this.minBuildings = 0;
			this.minBarracks = 0;
			this.minEmptyNodes = 0;
			this.minLandingPads = 0;
			this.minThroneRooms = 0;
			this.mainRect = CellRect.Empty;
			this.basePart_buildingsResolved = 0;
			this.basePart_emptyNodesResolved = 0;
			this.basePart_landingPadsResolved = 0;
			this.basePart_barracksResolved = 0;
			this.basePart_throneRoomsResolved = 0;
			this.basePart_batteriesCoverage = 0f;
			this.basePart_farmsCoverage = 0f;
			this.basePart_powerPlantsCoverage = 0f;
			this.basePart_breweriesCoverage = 0f;
		}

		// Token: 0x0600A707 RID: 42759 RVA: 0x0006E671 File Offset: 0x0006C871
		public void ClearResult()
		{
			this.landingPadsGenerated = 0;
		}

		// Token: 0x04007145 RID: 28997
		public Map map;

		// Token: 0x04007146 RID: 28998
		public int minBuildings;

		// Token: 0x04007147 RID: 28999
		public int minEmptyNodes;

		// Token: 0x04007148 RID: 29000
		public int minLandingPads;

		// Token: 0x04007149 RID: 29001
		public int minBarracks;

		// Token: 0x0400714A RID: 29002
		public int minThroneRooms;

		// Token: 0x0400714B RID: 29003
		public CellRect mainRect;

		// Token: 0x0400714C RID: 29004
		public int basePart_buildingsResolved;

		// Token: 0x0400714D RID: 29005
		public int basePart_emptyNodesResolved;

		// Token: 0x0400714E RID: 29006
		public int basePart_landingPadsResolved;

		// Token: 0x0400714F RID: 29007
		public int basePart_barracksResolved;

		// Token: 0x04007150 RID: 29008
		public int basePart_throneRoomsResolved;

		// Token: 0x04007151 RID: 29009
		public float basePart_batteriesCoverage;

		// Token: 0x04007152 RID: 29010
		public float basePart_farmsCoverage;

		// Token: 0x04007153 RID: 29011
		public float basePart_powerPlantsCoverage;

		// Token: 0x04007154 RID: 29012
		public float basePart_breweriesCoverage;

		// Token: 0x04007155 RID: 29013
		public int landingPadsGenerated;
	}
}
