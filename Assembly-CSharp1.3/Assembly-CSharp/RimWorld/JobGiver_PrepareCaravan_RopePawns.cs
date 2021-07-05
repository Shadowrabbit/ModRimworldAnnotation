using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078B RID: 1931
	public abstract class JobGiver_PrepareCaravan_RopePawns : ThinkNode_JobGiver
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060034FE RID: 13566
		protected abstract JobDef RopeJobDef { get; }

		// Token: 0x060034FF RID: 13567 RVA: 0x0012C2F0 File Offset: 0x0012A4F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Pawn pawn2;
			if (pawn.roping.IsRopingOthers)
			{
				pawn2 = pawn.roping.Ropees[0];
			}
			else
			{
				pawn2 = this.FindAnimalNeedingGathering(pawn);
			}
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.RopeJobDef, pawn2, cell);
			job.lord = pawn.GetLord();
			this.DecorateJob(job);
			return job;
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void DecorateJob(Job job)
		{
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0012C370 File Offset: 0x0012A570
		private Pawn FindAnimalNeedingGathering(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.GetLord().ownedPawns)
			{
				if (this.AnimalNeedsGathering(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x06003502 RID: 13570
		protected abstract bool AnimalNeedsGathering(Pawn pawn, Pawn animal);
	}
}
