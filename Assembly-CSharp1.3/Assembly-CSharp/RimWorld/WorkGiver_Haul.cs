using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000843 RID: 2115
	public abstract class WorkGiver_Haul : WorkGiver_Scanner
	{
		// Token: 0x06003807 RID: 14343 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x0013BC3D File Offset: 0x00139E3D
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x0013BC4F File Offset: 0x00139E4F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling().Count == 0;
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x0013BC69 File Offset: 0x00139E69
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, forced))
			{
				return null;
			}
			return HaulAIUtility.HaulToStorageJob(pawn, t);
		}
	}
}
