using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002113 RID: 8467
	public static class CaravanNameGenerator
	{
		// Token: 0x0600B3D1 RID: 46033 RVA: 0x0034377C File Offset: 0x0034197C
		public static string GenerateCaravanName(Caravan caravan)
		{
			Pawn pawn;
			if ((pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null)) == null)
			{
				pawn = (BestCaravanPawnUtility.FindBestDiplomat(caravan) ?? caravan.PawnsListForReading.Find((Pawn x) => caravan.IsOwner(x)));
			}
			Pawn pawn2 = pawn;
			TaggedString taggedString = (pawn2 != null) ? "CaravanLeaderCaravanName".Translate(pawn2.LabelShort, pawn2).CapitalizeFirst() : caravan.def.label;
			for (int i = 1; i <= 1000; i++)
			{
				TaggedString taggedString2 = taggedString;
				if (i != 1)
				{
					taggedString2 += " " + i;
				}
				if (!CaravanNameGenerator.CaravanNameInUse(taggedString2))
				{
					return taggedString2;
				}
			}
			Log.Error("Ran out of caravan names.", false);
			return caravan.def.label;
		}

		// Token: 0x0600B3D2 RID: 46034 RVA: 0x00343878 File Offset: 0x00341A78
		private static bool CaravanNameInUse(string name)
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}
	}
}
