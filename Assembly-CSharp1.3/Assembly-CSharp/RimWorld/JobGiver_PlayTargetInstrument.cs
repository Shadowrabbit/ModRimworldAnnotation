using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A1 RID: 1953
	public class JobGiver_PlayTargetInstrument : ThinkNode_JobGiver
	{
		// Token: 0x06003547 RID: 13639 RVA: 0x0012D74C File Offset: 0x0012B94C
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			Building_MusicalInstrument building_MusicalInstrument = ((duty != null) ? duty.focusSecond.Thing : null) as Building_MusicalInstrument;
			if (building_MusicalInstrument == null || !building_MusicalInstrument.Spawned)
			{
				return null;
			}
			if (!GatheringWorker_Concert.InstrumentAccessible(building_MusicalInstrument, pawn))
			{
				return null;
			}
			LordJob_Ritual lordJob_Ritual = pawn.GetLord().LordJob as LordJob_Ritual;
			Job job = JobMaker.MakeJob(JobDefOf.Play_MusicalInstrument, building_MusicalInstrument, building_MusicalInstrument.InteractionCell);
			job.doUntilGatheringEnded = true;
			if (lordJob_Ritual != null)
			{
				job.expiryInterval = lordJob_Ritual.DurationTicks;
			}
			else
			{
				job.expiryInterval = 2000;
			}
			return job;
		}
	}
}
