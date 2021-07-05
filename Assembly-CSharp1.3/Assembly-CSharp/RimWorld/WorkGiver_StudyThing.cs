using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200085D RID: 2141
	public class WorkGiver_StudyThing : WorkGiver_Scanner
	{
		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x0013D76E File Offset: 0x0013B96E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Studiable);
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x0600388B RID: 14475 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x0013D778 File Offset: 0x0013B978
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompStudiable compStudiable = t.TryGetComp<CompStudiable>();
			return compStudiable != null && !compStudiable.Completed && pawn.CanReserve(t, 1, -1, null, forced) && (!t.def.hasInteractionCell || pawn.CanReserveSittableOrSpot(t.InteractionCell, forced)) && !t.IsForbidden(pawn) && new HistoryEvent(HistoryEventDefOf.Researching, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job() && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Study) != null;
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0013D80D File Offset: 0x0013BA0D
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.StudyThing, t);
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0013D81F File Offset: 0x0013BA1F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Study);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x0013D839 File Offset: 0x0013BA39
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Studiable);
		}
	}
}
