using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCB RID: 3275
	public class JobGiver_TakeCombatEnhancingDrug : ThinkNode_JobGiver
	{
		// Token: 0x06004BB8 RID: 19384 RVA: 0x00035EEE File Offset: 0x000340EE
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCombatEnhancingDrug jobGiver_TakeCombatEnhancingDrug = (JobGiver_TakeCombatEnhancingDrug)base.DeepCopy(resolve);
			jobGiver_TakeCombatEnhancingDrug.onlyIfInDanger = this.onlyIfInDanger;
			return jobGiver_TakeCombatEnhancingDrug;
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x001A6AB8 File Offset: 0x001A4CB8
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

		// Token: 0x06004BBA RID: 19386 RVA: 0x00035F08 File Offset: 0x00034108
		private bool HarmedRecently(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < 2500;
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00035F27 File Offset: 0x00034127
		[Obsolete("Will be removed in a future update, use pawn.inventory.FindCombatEnhancingDrug()")]
		private Thing FindCombatEnhancingDrug(Pawn pawn)
		{
			return pawn.inventory.FindCombatEnhancingDrug();
		}

		// Token: 0x040031F6 RID: 12790
		private bool onlyIfInDanger;

		// Token: 0x040031F7 RID: 12791
		private const int TakeEveryTicks = 20000;
	}
}
