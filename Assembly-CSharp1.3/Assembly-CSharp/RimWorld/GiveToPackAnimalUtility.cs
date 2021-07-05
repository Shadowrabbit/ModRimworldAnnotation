using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006D3 RID: 1747
	public static class GiveToPackAnimalUtility
	{
		// Token: 0x060030B3 RID: 12467 RVA: 0x0011E4CC File Offset: 0x0011C6CC
		public static IEnumerable<Pawn> CarrierCandidatesFor(Pawn pawn)
		{
			IEnumerable<Pawn> enumerable = pawn.IsFormingCaravan() ? pawn.GetLord().ownedPawns : pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			enumerable = from x in enumerable
			where x.RaceProps.packAnimal && !x.inventory.UnloadEverything
			select x;
			if (pawn.Map.IsPlayerHome)
			{
				enumerable = from x in enumerable
				where x.IsFormingCaravan()
				select x;
			}
			return enumerable;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x0011E560 File Offset: 0x0011C760
		public static Pawn UsablePackAnimalWithTheMostFreeSpace(Pawn pawn)
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(pawn);
			Pawn pawn2 = null;
			float num = 0f;
			foreach (Pawn pawn3 in enumerable)
			{
				if (pawn3.RaceProps.packAnimal && pawn3 != pawn && pawn.CanReach(pawn3, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					float num2 = MassUtility.FreeSpace(pawn3);
					if (pawn2 == null || num2 > num)
					{
						pawn2 = pawn3;
						num = num2;
					}
				}
			}
			return pawn2;
		}
	}
}
