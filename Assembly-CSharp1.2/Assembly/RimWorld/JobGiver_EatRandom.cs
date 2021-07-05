using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class JobGiver_EatRandom : ThinkNode_JobGiver
	{
		// Token: 0x06004AAA RID: 19114 RVA: 0x001A26B8 File Offset: 0x001A08B8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			Predicate<Thing> validator = (Thing t) => t.def.category == ThingCategory.Item && t.IngestibleNow && pawn.RaceProps.CanEverEat(t) && pawn.CanReserve(t, 1, -1, null, false);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Ingest, thing);
		}
	}
}
