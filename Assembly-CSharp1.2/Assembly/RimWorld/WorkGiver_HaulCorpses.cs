using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D83 RID: 3459
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		// Token: 0x06004EE4 RID: 20196 RVA: 0x00037923 File Offset: 0x00035B23
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Corpse))
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x001B35B0 File Offset: 0x001B17B0
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
