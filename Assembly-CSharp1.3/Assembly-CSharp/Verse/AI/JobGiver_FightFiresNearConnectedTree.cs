using System;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200064F RID: 1615
	public class JobGiver_FightFiresNearConnectedTree : ThinkNode_JobGiver
	{
		// Token: 0x06002DB8 RID: 11704 RVA: 0x00110C54 File Offset: 0x0010EE54
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = pawn.connections.ConnectedThings.FirstOrDefault((Thing x) => x.Spawned && x.Map == pawn.Map && pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
			if (thing == null)
			{
				return null;
			}
			Thing thing2 = GenClosest.ClosestThingReachable(thing.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 10f, JobGiver_FightFiresNearPoint.FireValidator(pawn), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing2 != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing2);
			}
			return null;
		}

		// Token: 0x04001BEA RID: 7146
		private const float FightFireDistance = 10f;
	}
}
