using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B8 RID: 1976
	public class JobGiver_TakeCombatEnhancingDrug : ThinkNode_JobGiver
	{
		// Token: 0x06003589 RID: 13705 RVA: 0x0012EAD7 File Offset: 0x0012CCD7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCombatEnhancingDrug jobGiver_TakeCombatEnhancingDrug = (JobGiver_TakeCombatEnhancingDrug)base.DeepCopy(resolve);
			jobGiver_TakeCombatEnhancingDrug.onlyIfInDanger = this.onlyIfInDanger;
			return jobGiver_TakeCombatEnhancingDrug;
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x0012EAF4 File Offset: 0x0012CCF4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.IsTeetotaler())
			{
				return null;
			}
			if (Find.TickManager.TicksGame - pawn.mindState.lastTakeCombatEnhancingDrugTick < 20000)
			{
				return null;
			}
			Thing thing = pawn.inventory.FindCombatEnhancingDrug();
			if (thing == null)
			{
				return null;
			}
			if (this.onlyIfInDanger)
			{
				Lord lord = pawn.GetLord();
				if (lord == null)
				{
					if (!this.HarmedRecently(pawn))
					{
						return null;
					}
				}
				else
				{
					int num = 0;
					int num2 = Mathf.Clamp(lord.ownedPawns.Count / 2, 1, 4);
					for (int i = 0; i < lord.ownedPawns.Count; i++)
					{
						if (this.HarmedRecently(lord.ownedPawns[i]))
						{
							num++;
							if (num >= num2)
							{
								break;
							}
						}
					}
					if (num < num2)
					{
						return null;
					}
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = 1;
			return job;
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x0012EBC3 File Offset: 0x0012CDC3
		private bool HarmedRecently(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < 2500;
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x0012EBE2 File Offset: 0x0012CDE2
		[Obsolete("Will be removed in a future update, use pawn.inventory.FindCombatEnhancingDrug()")]
		private Thing FindCombatEnhancingDrug(Pawn pawn)
		{
			return pawn.inventory.FindCombatEnhancingDrug();
		}

		// Token: 0x04001EA3 RID: 7843
		private bool onlyIfInDanger;

		// Token: 0x04001EA4 RID: 7844
		private const int TakeEveryTicks = 20000;
	}
}
