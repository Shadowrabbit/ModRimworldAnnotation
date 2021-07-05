using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA3 RID: 2723
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		// Token: 0x0600408A RID: 16522 RVA: 0x0003046C File Offset: 0x0002E66C
		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00182E30 File Offset: 0x00181030
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
				if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
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
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 35f, validator, null, 13, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return thing.Position;
			}
			return pawn.Position;
		}

		// Token: 0x04002C6B RID: 11371
		private const int MinDistToHumanlike = 15;
	}
}
