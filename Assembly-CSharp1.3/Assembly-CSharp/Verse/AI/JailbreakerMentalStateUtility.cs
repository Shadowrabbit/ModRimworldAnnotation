using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C0 RID: 1472
	public static class JailbreakerMentalStateUtility
	{
		// Token: 0x06002AEB RID: 10987 RVA: 0x001014E4 File Offset: 0x000FF6E4
		public static Pawn FindPrisoner(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			JailbreakerMentalStateUtility.tmpPrisoners.Clear();
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction && pawn2 != pawn && !pawn2.Downed && !pawn2.InMentalState && !pawn2.IsBurning() && pawn2.Awake() && pawn2.guest.PrisonerIsSecure && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn2) && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					JailbreakerMentalStateUtility.tmpPrisoners.Add(pawn2);
				}
			}
			if (!JailbreakerMentalStateUtility.tmpPrisoners.Any<Pawn>())
			{
				return null;
			}
			Pawn result = JailbreakerMentalStateUtility.tmpPrisoners.RandomElement<Pawn>();
			JailbreakerMentalStateUtility.tmpPrisoners.Clear();
			return result;
		}

		// Token: 0x04001A59 RID: 6745
		private static List<Pawn> tmpPrisoners = new List<Pawn>();
	}
}
