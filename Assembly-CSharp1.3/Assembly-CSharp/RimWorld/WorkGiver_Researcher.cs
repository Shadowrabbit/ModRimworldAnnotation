using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200085E RID: 2142
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06003891 RID: 14481 RVA: 0x0013D84D File Offset: 0x0013BA4D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (Find.ResearchManager.currentProj == null)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Nothing);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.ResearchBench);
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06003892 RID: 14482 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x0013D869 File Offset: 0x0013BA69
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return Find.ResearchManager.currentProj == null;
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0013D87C File Offset: 0x0013BA7C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj == null)
			{
				return false;
			}
			Building_ResearchBench building_ResearchBench = t as Building_ResearchBench;
			return building_ResearchBench != null && currentProj.CanBeResearchedAt(building_ResearchBench, false) && pawn.CanReserve(t, 1, -1, null, forced) && (!t.def.hasInteractionCell || pawn.CanReserveSittableOrSpot(t.InteractionCell, forced)) && new HistoryEvent(HistoryEventDefOf.Researching, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0013D900 File Offset: 0x0013BB00
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Research, t);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0013D912 File Offset: 0x0013BB12
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
		}
	}
}
