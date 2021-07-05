using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200082B RID: 2091
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x001391FF File Offset: 0x001373FF
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x00139218 File Offset: 0x00137418
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.interactionMode != PrisonerInteractionModeDefOf.Execution || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!base.IsExecutionIdeoAllowed(pawn, pawn2))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerExecution, t);
		}

		// Token: 0x04001F15 RID: 7957
		private static string IncapableOfViolenceLowerTrans;
	}
}
