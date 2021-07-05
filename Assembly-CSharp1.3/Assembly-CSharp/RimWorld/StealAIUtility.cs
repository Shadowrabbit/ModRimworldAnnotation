using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B6 RID: 1974
	public static class StealAIUtility
	{
		// Token: 0x0600357E RID: 13694 RVA: 0x0012E4CC File Offset: 0x0012C6CC
		public static bool TryFindBestItemToSteal(IntVec3 root, Map map, float maxDist, out Thing item, Pawn thief, List<Thing> disallowed = null)
		{
			if (map == null)
			{
				item = null;
				return false;
			}
			if (thief != null && !thief.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				item = null;
				return false;
			}
			if ((thief != null && !map.reachability.CanReachMapEdge(thief.Position, TraverseParms.For(thief, Danger.Some, TraverseMode.ByPawn, false, false, false))) || (thief == null && !map.reachability.CanReachMapEdge(root, TraverseParms.For(TraverseMode.PassDoors, Danger.Some, false, false, false))))
			{
				item = null;
				return false;
			}
			Predicate<Thing> validator = (Thing t) => (thief == null || thief.CanReserve(t, 1, -1, null, false)) && (disallowed == null || !disallowed.Contains(t)) && t.def.stealable && !t.IsBurning();
			item = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(root, map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false, false, false), maxDist, validator, (Thing x) => StealAIUtility.GetValue(x), 15, 15);
			if (item != null && StealAIUtility.GetValue(item) < 320f)
			{
				item = null;
			}
			return item != null;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x0012E5DC File Offset: 0x0012C7DC
		public static float TotalMarketValueAround(List<Pawn> pawns)
		{
			float num = 0f;
			StealAIUtility.tmpToSteal.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				Thing thing;
				if (pawns[i].Spawned && StealAIUtility.TryFindBestItemToSteal(pawns[i].Position, pawns[i].Map, 7f, out thing, pawns[i], StealAIUtility.tmpToSteal))
				{
					num += StealAIUtility.GetValue(thing);
					StealAIUtility.tmpToSteal.Add(thing);
				}
			}
			StealAIUtility.tmpToSteal.Clear();
			return num;
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x0012E66C File Offset: 0x0012C86C
		public static float StartStealingMarketValueThreshold(Lord lord)
		{
			Rand.PushState();
			Rand.Seed = lord.loadID;
			float randomInRange = StealAIUtility.StealThresholdValuePerCombatPowerRange.RandomInRange;
			Rand.PopState();
			float num = 0f;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				num += Mathf.Max(lord.ownedPawns[i].kindDef.combatPower, 100f);
			}
			return num * randomInRange;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0012E6DE File Offset: 0x0012C8DE
		public static float GetValue(Thing thing)
		{
			return thing.MarketValue * (float)thing.stackCount;
		}

		// Token: 0x04001E9C RID: 7836
		private const float MinMarketValueToTake = 320f;

		// Token: 0x04001E9D RID: 7837
		private static readonly FloatRange StealThresholdValuePerCombatPowerRange = new FloatRange(2f, 10f);

		// Token: 0x04001E9E RID: 7838
		private const float MinCombatPowerPerPawn = 100f;

		// Token: 0x04001E9F RID: 7839
		private static List<Thing> tmpToSteal = new List<Thing>();
	}
}
