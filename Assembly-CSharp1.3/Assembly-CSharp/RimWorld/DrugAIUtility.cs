using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000772 RID: 1906
	public static class DrugAIUtility
	{
		// Token: 0x0600348A RID: 13450 RVA: 0x00129D98 File Offset: 0x00127F98
		public static Job IngestAndTakeToInventoryJob(Thing drug, Pawn pawn, int maxNumToCarry = 9999)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, drug);
			job.count = Mathf.Min(new int[]
			{
				drug.stackCount,
				drug.def.ingestible.maxNumToIngestAtOnce,
				maxNumToCarry
			});
			if (pawn.drugs != null)
			{
				DrugPolicyEntry drugPolicyEntry = pawn.drugs.CurrentPolicy[drug.def];
				int num = pawn.inventory.innerContainer.TotalStackCountOfDef(drug.def) - job.count;
				if (drugPolicyEntry.allowScheduled && num <= 0)
				{
					job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
				}
			}
			return job;
		}
	}
}
