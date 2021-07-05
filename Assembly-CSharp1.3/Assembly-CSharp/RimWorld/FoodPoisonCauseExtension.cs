using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001136 RID: 4406
	public static class FoodPoisonCauseExtension
	{
		// Token: 0x060069D8 RID: 27096 RVA: 0x0023A75C File Offset: 0x0023895C
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
