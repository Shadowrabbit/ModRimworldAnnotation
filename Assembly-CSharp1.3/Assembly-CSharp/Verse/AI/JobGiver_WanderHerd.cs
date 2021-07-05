using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000642 RID: 1602
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		// Token: 0x06002D95 RID: 11669 RVA: 0x00110608 File Offset: 0x0010E808
		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x00110630 File Offset: 0x0010E830
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (((Pawn)t).RaceProps != pawn.RaceProps || t == pawn)
				{
					return false;
				}
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
				if (t.Position.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return false;
				}
				if (Rand.Value < 0.5f)
				{
					return false;
				}
				List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2.RaceProps.Humanlike && (pawn2.Position - t.Position).LengthHorizontalSquared < 225)
					{
						return false;
					}
				}
				return true;
			};
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 35f, validator, null, 13, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return thing.Position;
			}
			return pawn.Position;
		}

		// Token: 0x04001BE7 RID: 7143
		private const int MinDistToHumanlike = 15;
	}
}
