using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007B5 RID: 1973
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		// Token: 0x0600357C RID: 13692 RVA: 0x0012E440 File Offset: 0x0012C640
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				return null;
			}
			Thing thing;
			if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 12f, out thing, pawn, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Steal);
				job.targetA = thing;
				job.targetB = c;
				job.count = Mathf.Min(thing.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / thing.def.VolumePerUnit));
				return job;
			}
			return null;
		}

		// Token: 0x04001E9A RID: 7834
		public const float ItemsSearchRadiusInitial = 7f;

		// Token: 0x04001E9B RID: 7835
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
