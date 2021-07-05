using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017A0 RID: 6048
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x06008C27 RID: 35879 RVA: 0x003243F1 File Offset: 0x003225F1
		public static void CheckTakeScheduledDrugs(Caravan caravan)
		{
			if (caravan.IsHashIntervalTick(120))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(caravan);
			}
		}

		// Token: 0x06008C28 RID: 35880 RVA: 0x00324404 File Offset: 0x00322604
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x06008C29 RID: 35881 RVA: 0x00324438 File Offset: 0x00322638
		private static void TryTakeScheduledDrugs(Pawn pawn, Caravan caravan)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				Thing drug;
				Pawn drugOwner;
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug) && CaravanInventoryUtility.TryGetThingOfDef(caravan, currentPolicy[i].drug, out drug, out drugOwner))
				{
					caravan.needs.IngestDrug(pawn, drug, drugOwner);
				}
			}
		}

		// Token: 0x04005902 RID: 22786
		private const int TryTakeScheduledDrugsIntervalTicks = 120;
	}
}
