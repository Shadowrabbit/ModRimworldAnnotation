using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C9 RID: 1481
	public class MentalStateWorker_BingingDrug : MentalStateWorker
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x00102A78 File Offset: 0x00100C78
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], this.def.drugCategory))
				{
					return true;
				}
				if (this.def.drugCategory == DrugCategory.Hard && AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], DrugCategory.Social))
				{
					return true;
				}
			}
			return false;
		}
	}
}
