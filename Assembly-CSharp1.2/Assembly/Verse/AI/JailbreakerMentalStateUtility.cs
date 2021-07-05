using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A15 RID: 2581
	public static class JailbreakerMentalStateUtility
	{
		// Token: 0x06003DA4 RID: 15780 RVA: 0x00176110 File Offset: 0x00174310
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
				if (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction && pawn2 != pawn && !pawn2.Downed && !pawn2.InMentalState && !pawn2.IsBurning() && pawn2.Awake() && pawn2.guest.PrisonerIsSecure && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn2) && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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

		// Token: 0x04002ABB RID: 10939
		private static List<Pawn> tmpPrisoners = new List<Pawn>();
	}
}
