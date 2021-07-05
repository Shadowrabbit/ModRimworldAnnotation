using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D61 RID: 3425
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004E40 RID: 20032 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x000373D1 File Offset: 0x000355D1
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x001B0BEC File Offset: 0x001AEDEC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner_NewTemp(pawn, t, forced))
			{
				return null;
			}
			if (((Pawn)t).guest.interactionMode != PrisonerInteractionModeDefOf.Execution || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerExecution, t);
		}

		// Token: 0x0400331A RID: 13082
		private static string IncapableOfViolenceLowerTrans;
	}
}
