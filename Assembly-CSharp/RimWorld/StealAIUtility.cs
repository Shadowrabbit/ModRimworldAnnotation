using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC7 RID: 3271
	public static class StealAIUtility
	{
		// Token: 0x06004BA8 RID: 19368 RVA: 0x001A64CC File Offset: 0x001A46CC
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
			if ((thief != null && !map.reachability.CanReachMapEdge(thief.Position, TraverseParms.For(thief, Danger.Some, TraverseMode.ByPawn, false))) || (thief == null && !map.reachability.CanReachMapEdge(root, TraverseParms.For(TraverseMode.PassDoors, Danger.Some, false))))
			{
				item = null;
				return false;
			}
			Predicate<Thing> validator = (Thing t) => (thief == null || thief.CanReserve(t, 1, -1, null, false)) && (disallowed == null || !disallowed.Contains(t)) && t.def.stealable && !t.IsBurning();
			item = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(root, map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, (Thing x) => StealAIUtility.GetValue(x), 15, 15);
			if (item != null && StealAIUtility.GetValue(item) < 320f)
			{
				item = null;
			}
			return item != null;
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x001A65D8 File Offset: 0x001A47D8
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

		// Token: 0x06004BAA RID: 19370 RVA: 0x001A6668 File Offset: 0x001A4868
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

		// Token: 0x06004BAB RID: 19371 RVA: 0x00035E5F File Offset: 0x0003405F
		public static float GetValue(Thing thing)
		{
			return thing.MarketValue * (float)thing.stackCount;
		}

		// Token: 0x040031EB RID: 12779
		private const float MinMarketValueToTake = 320f;

		// Token: 0x040031EC RID: 12780
		private static readonly FloatRange StealThresholdValuePerCombatPowerRange = new FloatRange(2f, 10f);

		// Token: 0x040031ED RID: 12781
		private const float MinCombatPowerPerPawn = 100f;

		// Token: 0x040031EE RID: 12782
		private static List<Thing> tmpToSteal = new List<Thing>();
	}
}
