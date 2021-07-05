using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B0 RID: 4272
	public class Medicine : ThingWithComps
	{
		// Token: 0x06006610 RID: 26128 RVA: 0x00227560 File Offset: 0x00225760
		public static int GetMedicineCountToFullyHeal(Pawn pawn)
		{
			int num = 0;
			int num2 = pawn.health.hediffSet.hediffs.Count + 1;
			Medicine.tendableHediffsInTendPriorityOrder.Clear();
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].TendableNow(false))
				{
					Medicine.tendableHediffsInTendPriorityOrder.Add(hediffs[i]);
				}
			}
			TendUtility.SortByTendPriority(Medicine.tendableHediffsInTendPriorityOrder);
			int num3 = 0;
			for (;;)
			{
				num++;
				if (num > num2)
				{
					break;
				}
				TendUtility.GetOptimalHediffsToTendWithSingleTreatment(pawn, true, Medicine.tmpHediffs, Medicine.tendableHediffsInTendPriorityOrder);
				if (!Medicine.tmpHediffs.Any<Hediff>())
				{
					goto IL_DF;
				}
				num3++;
				for (int j = 0; j < Medicine.tmpHediffs.Count; j++)
				{
					Medicine.tendableHediffsInTendPriorityOrder.Remove(Medicine.tmpHediffs[j]);
				}
			}
			Log.Error("Too many iterations.");
			IL_DF:
			Medicine.tmpHediffs.Clear();
			Medicine.tendableHediffsInTendPriorityOrder.Clear();
			return num3;
		}

		// Token: 0x04003997 RID: 14743
		private static List<Hediff> tendableHediffsInTendPriorityOrder = new List<Hediff>();

		// Token: 0x04003998 RID: 14744
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
