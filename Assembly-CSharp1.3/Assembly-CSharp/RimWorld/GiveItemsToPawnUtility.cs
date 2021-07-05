using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C8 RID: 2248
	public static class GiveItemsToPawnUtility
	{
		// Token: 0x06003B39 RID: 15161 RVA: 0x0014ADB4 File Offset: 0x00148FB4
		public static Thing FindItemToGive(Pawn hauler, ThingDef thingDef)
		{
			return GenClosest.ClosestThingReachable(hauler.Position, hauler.Map, ThingRequest.ForDef(thingDef), PathEndMode.Touch, TraverseParms.For(hauler, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => !x.IsForbidden(hauler) && hauler.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x0014AE18 File Offset: 0x00149018
		public static bool IsWaitingForItems(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.CurLordToil is LordToil_WaitForItems;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x0014AE3F File Offset: 0x0014903F
		public static int ItemCountWaitingFor(Pawn pawn)
		{
			if (!GiveItemsToPawnUtility.IsWaitingForItems(pawn))
			{
				return -1;
			}
			return ((LordToil_WaitForItems)pawn.GetLord().CurLordToil).CountRemaining;
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x0014AE60 File Offset: 0x00149060
		public static int ItemCountBeingHauled(Pawn requester)
		{
			List<Pawn> allPawnsSpawned = requester.Map.mapPawns.AllPawnsSpawned;
			Lord lord = requester.GetLord();
			int num = 0;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				JobDriver_GiveToPawn jobDriver_GiveToPawn;
				if (pawn.CurJob != null && (jobDriver_GiveToPawn = (pawn.jobs.curDriver as JobDriver_GiveToPawn)) != null && jobDriver_GiveToPawn.job.lord == lord)
				{
					num += jobDriver_GiveToPawn.CountBeingHauled;
				}
			}
			return num;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x0014AEDA File Offset: 0x001490DA
		public static int ItemCountLeftToCollect(Pawn requester)
		{
			return GiveItemsToPawnUtility.ItemCountWaitingFor(requester) - GiveItemsToPawnUtility.ItemCountBeingHauled(requester);
		}
	}
}
