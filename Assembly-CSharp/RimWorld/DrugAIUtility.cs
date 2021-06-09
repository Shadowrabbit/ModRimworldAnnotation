using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C8F RID: 3215
	public static class DrugAIUtility
	{
		// Token: 0x06004AFF RID: 19199 RVA: 0x001A3D24 File Offset: 0x001A1F24
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
