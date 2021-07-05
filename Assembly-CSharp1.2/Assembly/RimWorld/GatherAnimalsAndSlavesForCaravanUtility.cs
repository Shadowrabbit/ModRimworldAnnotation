using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE3 RID: 3555
	public static class GatherAnimalsAndSlavesForCaravanUtility
	{
		// Token: 0x06005110 RID: 20752 RVA: 0x0000A2E4 File Offset: 0x000084E4
		[Obsolete]
		public static bool IsFollowingAnyone(Pawn p)
		{
			return false;
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete]
		public static void SetFollower(Pawn p, Pawn follower)
		{
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x001BA5F4 File Offset: 0x001B87F4
		public static void CheckArrived(Lord lord, List<Pawn> pawns, IntVec3 meetingPoint, string memo, Predicate<Pawn> shouldCheckIfArrived, Predicate<Pawn> extraValidator = null)
		{
			bool flag = true;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (shouldCheckIfArrived(pawn) && (!pawn.Spawned || !pawn.Position.InHorDistOf(meetingPoint, 10f) || !pawn.CanReach(meetingPoint, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) || (extraValidator != null && !extraValidator(pawn))))
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
	}
}
