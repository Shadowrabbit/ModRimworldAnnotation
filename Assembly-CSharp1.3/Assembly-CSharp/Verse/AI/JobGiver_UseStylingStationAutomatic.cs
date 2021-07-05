using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A2 RID: 1442
	public class JobGiver_UseStylingStationAutomatic : ThinkNode_JobGiver
	{
		// Token: 0x060029FB RID: 10747 RVA: 0x000FD1C0 File Offset: 0x000FB3C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < pawn.style.nextStyleChangeAttemptTick)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.StylingStation), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null);
			if (thing == null)
			{
				pawn.style.ResetNextStyleChangeAttemptTick();
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UseStylingStationAutomatic, thing);
		}
	}
}
