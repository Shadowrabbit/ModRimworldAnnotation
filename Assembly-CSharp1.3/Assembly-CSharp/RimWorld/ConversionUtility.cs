using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D28 RID: 3368
	public static class ConversionUtility
	{
		// Token: 0x06004F02 RID: 20226 RVA: 0x001A7890 File Offset: 0x001A5A90
		public static TaggedString GetCertaintyReductionFactorsDescription(Pawn pawn)
		{
			TaggedString taggedString = string.Empty;
			if (pawn.Ideo != null)
			{
				float num = 1f;
				List<Precept> preceptsListForReading = pawn.Ideo.PreceptsListForReading;
				for (int i = 0; i < preceptsListForReading.Count; i++)
				{
					if (preceptsListForReading[i].def.statFactors != null)
					{
						num *= preceptsListForReading[i].def.statFactors.GetStatFactorFromList(StatDefOf.CertaintyLossFactor);
					}
				}
				if (num != 1f)
				{
					taggedString += "\n -  " + "AbilityIdeoConvertBreakdownIdeoCertaintyReduction".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO")) + ": " + num.ToStringPercent();
				}
			}
			foreach (Trait trait in pawn.story.traits.allTraits)
			{
				float num2 = trait.MultiplierOfStat(StatDefOf.CertaintyLossFactor);
				if (num2 != 1f)
				{
					taggedString += "\n -  " + "AbilityIdeoConvertBreakdownTrait".Translate(pawn.Named("PAWN"), trait.Label.ToLower().Named("TRAIT")) + ": x" + num2.ToStringPercent();
				}
			}
			return taggedString;
		}
	}
}
