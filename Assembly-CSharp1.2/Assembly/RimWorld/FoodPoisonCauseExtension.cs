using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C0 RID: 6080
	public static class FoodPoisonCauseExtension
	{
		// Token: 0x06008676 RID: 34422 RVA: 0x00278ED4 File Offset: 0x002770D4
		public static string ToStringHuman(this FoodPoisonCause cause)
		{
			switch (cause)
			{
			case FoodPoisonCause.Unknown:
				return "UnknownLower".Translate().CapitalizeFirst();
			case FoodPoisonCause.IncompetentCook:
				return "FoodPoisonCause_IncompetentCook".Translate();
			case FoodPoisonCause.FilthyKitchen:
				return "FoodPoisonCause_FilthyKitchen".Translate();
			case FoodPoisonCause.Rotten:
				return "FoodPoisonCause_Rotten".Translate();
			case FoodPoisonCause.DangerousFoodType:
				return "FoodPoisonCause_DangerousFoodType".Translate();
			default:
				return cause.ToString();
			}
		}
	}
}
