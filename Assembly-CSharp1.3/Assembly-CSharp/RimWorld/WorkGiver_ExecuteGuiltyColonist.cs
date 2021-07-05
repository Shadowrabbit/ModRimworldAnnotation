using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000826 RID: 2086
	public class WorkGiver_ExecuteGuiltyColonist : WorkGiver_Scanner
	{
		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x00138AD6 File Offset: 0x00136CD6
		public static void ResetStaticData()
		{
			WorkGiver_ExecuteGuiltyColonist.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x00138AEC File Offset: 0x00136CEC
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.FreeColonistsSpawned;
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x00138AFE File Offset: 0x00136CFE
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.FreeColonistsSpawnedCount == 0;
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x00138B14 File Offset: 0x00136D14
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			if (pawn2 == null || pawn2.guilt == null || !pawn2.guilt.IsGuilty || !pawn2.guilt.awaitingExecution || !pawn2.Spawned || pawn2.IsForbidden(pawn) || pawn2.IsFormingCaravan() || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_ExecuteGuiltyColonist.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!new HistoryEvent(HistoryEventDefOf.ExecutedColonist, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GuiltyColonistExecution, t);
		}

		// Token: 0x04001F14 RID: 7956
		private static string IncapableOfViolenceLowerTrans;
	}
}
