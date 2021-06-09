using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CC6 RID: 3270
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		// Token: 0x06004BA6 RID: 19366 RVA: 0x001A6440 File Offset: 0x001A4640
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

		// Token: 0x040031E9 RID: 12777
		public const float ItemsSearchRadiusInitial = 7f;

		// Token: 0x040031EA RID: 12778
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
