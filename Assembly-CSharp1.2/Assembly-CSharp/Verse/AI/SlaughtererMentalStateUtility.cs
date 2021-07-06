using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4D RID: 2637
	public static class SlaughtererMentalStateUtility
	{
		// Token: 0x06003EA3 RID: 16035 RVA: 0x00178C18 File Offset: 0x00176E18
		public static Pawn FindAnimal(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			SlaughtererMentalStateUtility.tmpAnimals.Clear();
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2.RaceProps.Animal && pawn2.Faction == pawn.Faction && pawn2 != pawn && !pawn2.IsBurning() && !pawn2.InAggroMentalState && pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
				{
					SlaughtererMentalStateUtility.tmpAnimals.Add(pawn2);
				}
			}
			if (!SlaughtererMentalStateUtility.tmpAnimals.Any<Pawn>())
			{
				return null;
			}
			Pawn result = SlaughtererMentalStateUtility.tmpAnimals.RandomElement<Pawn>();
			SlaughtererMentalStateUtility.tmpAnimals.Clear();
			return result;
		}

		// Token: 0x04002B0F RID: 11023
		private static List<Pawn> tmpAnimals = new List<Pawn>();
	}
}
