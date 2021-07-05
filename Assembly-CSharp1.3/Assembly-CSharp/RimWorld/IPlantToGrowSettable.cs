using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0F RID: 3343
	public interface IPlantToGrowSettable
	{
		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06004E34 RID: 20020
		Map Map { get; }

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06004E35 RID: 20021
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x06004E36 RID: 20022
		ThingDef GetPlantDefToGrow();

		// Token: 0x06004E37 RID: 20023
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x06004E38 RID: 20024
		bool CanAcceptSowNow();
	}
}
