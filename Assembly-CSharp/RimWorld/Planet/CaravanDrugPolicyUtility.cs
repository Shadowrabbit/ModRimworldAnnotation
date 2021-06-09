using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020E0 RID: 8416
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x0600B2D4 RID: 45780 RVA: 0x0007434A File Offset: 0x0007254A
		public static void CheckTakeScheduledDrugs(Caravan caravan)
		{
			if (caravan.IsHashIntervalTick(120))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(caravan);
			}
		}

		// Token: 0x0600B2D5 RID: 45781 RVA: 0x0033D13C File Offset: 0x0033B33C
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x0600B2D6 RID: 45782 RVA: 0x0033D170 File Offset: 0x0033B370
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

		// Token: 0x04007AF2 RID: 31474
		private const int TryTakeScheduledDrugsIntervalTicks = 120;
	}
}
