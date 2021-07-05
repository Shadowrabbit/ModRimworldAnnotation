using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A1 RID: 2209
	public static class GatherAnimalsAndSlavesForCaravanUtility
	{
		// Token: 0x06003A7C RID: 14972 RVA: 0x0014764C File Offset: 0x0014584C
		public static void CheckArrived(Lord lord, List<Pawn> pawns, IntVec3 meetingPoint, string memo, Predicate<Pawn> shouldCheckIfArrived, Predicate<Pawn> extraValidator = null)
		{
			bool flag = true;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (shouldCheckIfArrived(pawn) && (!pawn.Spawned || !pawn.Position.InHorDistOf(meetingPoint, 10f) || !pawn.CanReach(meetingPoint, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) || (extraValidator != null && !extraValidator(pawn))))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				lord.ReceiveMemo(memo);
			}
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x001476CC File Offset: 0x001458CC
		public static bool IsRopedByCaravanPawn(Pawn animal)
		{
			Lord lord = animal.GetLord();
			if (lord != null && animal.roping.IsRopedByPawn)
			{
				Pawn ropedByPawn = animal.roping.RopedByPawn;
				return ((ropedByPawn != null) ? ropedByPawn.GetLord() : null) == lord;
			}
			return false;
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x0014770C File Offset: 0x0014590C
		public static bool CanRoperTakeAnimalToDest(Pawn pawn, Pawn animal, IntVec3 destSpot)
		{
			return pawn.CanReach(animal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && animal.Map.reachability.CanReach(animal.Position, destSpot, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false).WithFenceblockedOf(animal));
		}
	}
}
