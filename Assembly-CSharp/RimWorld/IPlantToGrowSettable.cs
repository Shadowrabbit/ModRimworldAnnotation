using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134B RID: 4939
	public interface IPlantToGrowSettable
	{
		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06006B2D RID: 27437
		Map Map { get; }

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06006B2E RID: 27438
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x06006B2F RID: 27439
		ThingDef GetPlantDefToGrow();

		// Token: 0x06006B30 RID: 27440
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x06006B31 RID: 27441
		bool CanAcceptSowNow();
	}
}
