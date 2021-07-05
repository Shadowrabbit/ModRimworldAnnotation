using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000938 RID: 2360
	public class ThoughtWorker_Precept_SlabBed_Preferred : ThoughtWorker_Precept
	{
		// Token: 0x06003CBD RID: 15549 RVA: 0x0015017D File Offset: 0x0014E37D
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.mindState.lastBedDefSleptIn != null && p.mindState.lastBedDefSleptIn.building != null && p.mindState.lastBedDefSleptIn.building.bed_slabBed;
		}
	}
}
