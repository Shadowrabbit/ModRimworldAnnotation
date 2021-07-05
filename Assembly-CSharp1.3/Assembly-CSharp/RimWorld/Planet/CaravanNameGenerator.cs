using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017BC RID: 6076
	public static class CaravanNameGenerator
	{
		// Token: 0x06008CFF RID: 36095 RVA: 0x0032BEC8 File Offset: 0x0032A0C8
		public static string GenerateCaravanName(Caravan caravan)
		{
			Pawn pawn;
			if ((pawn = IdeoUtility.FindFirstPawnWithLeaderRole(caravan)) == null && (pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null)) == null)
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
			Log.Error("Ran out of caravan names.");
			return caravan.def.label;
		}

		// Token: 0x06008D00 RID: 36096 RVA: 0x0032BFD4 File Offset: 0x0032A1D4
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
