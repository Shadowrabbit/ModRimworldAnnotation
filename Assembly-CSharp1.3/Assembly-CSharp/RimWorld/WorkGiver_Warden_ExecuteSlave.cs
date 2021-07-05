using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200082E RID: 2094
	public class WorkGiver_Warden_ExecuteSlave : WorkGiver_Warden
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600378C RID: 14220 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x001393E7 File Offset: 0x001375E7
		public static void ResetStaticData()
		{
			WorkGiver_Warden_ExecuteSlave.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00139400 File Offset: 0x00137600
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfSlave(pawn, t, forced))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Execute || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_ExecuteSlave.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!base.IsExecutionIdeoAllowed(pawn, pawn2))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.SlaveExecution, t);
		}

		// Token: 0x04001F16 RID: 7958
		private static string IncapableOfViolenceLowerTrans;
	}
}
