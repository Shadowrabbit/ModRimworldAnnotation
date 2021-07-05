using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000845 RID: 2117
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		// Token: 0x0600380E RID: 14350 RVA: 0x0013BC9B File Offset: 0x00139E9B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Corpse))
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x0013BCB0 File Offset: 0x00139EB0
		public override string PostProcessedGerund(Job job)
		{
			if (job.GetTarget(TargetIndex.B).Thing is Building_Grave)
			{
				return "Burying".Translate();
			}
			return base.PostProcessedGerund(job);
		}
	}
}
