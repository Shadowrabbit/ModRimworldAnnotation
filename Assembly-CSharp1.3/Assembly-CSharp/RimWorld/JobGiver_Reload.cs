using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000825 RID: 2085
	public class JobGiver_Reload : ThinkNode_JobGiver
	{
		// Token: 0x06003769 RID: 14185 RVA: 0x001389F7 File Offset: 0x00136BF7
		public override float GetPriority(Pawn pawn)
		{
			return 5.9f;
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x00138A00 File Offset: 0x00136C00
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompReloadable compReloadable = ReloadableUtility.FindSomeReloadableComponent(pawn, false);
			if (compReloadable == null)
			{
				return null;
			}
			List<Thing> list = ReloadableUtility.FindEnoughAmmo(pawn, pawn.Position, compReloadable, false);
			if (list == null)
			{
				return null;
			}
			return JobGiver_Reload.MakeReloadJob(compReloadable, list);
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x00138A38 File Offset: 0x00136C38
		public static Job MakeReloadJob(CompReloadable comp, List<Thing> chosenAmmo)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Reload, comp.parent);
			job.targetQueueB = (from t in chosenAmmo
			select new LocalTargetInfo(t)).ToList<LocalTargetInfo>();
			job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
			job.count = Math.Min(job.count, comp.MaxAmmoNeeded(true));
			return job;
		}

		// Token: 0x04001F13 RID: 7955
		private const bool forceReloadWhenLookingForWork = false;
	}
}
