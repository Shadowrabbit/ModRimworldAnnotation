using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5C RID: 3420
	public class JobGiver_Reload : ThinkNode_JobGiver
	{
		// Token: 0x06004E2B RID: 20011 RVA: 0x0003736B File Offset: 0x0003556B
		public override float GetPriority(Pawn pawn)
		{
			return 5.9f;
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x001B06F0 File Offset: 0x001AE8F0
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

		// Token: 0x06004E2D RID: 20013 RVA: 0x001B0728 File Offset: 0x001AE928
		public static Job MakeReloadJob(CompReloadable comp, List<Thing> chosenAmmo)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Reload, comp.parent);
			job.targetQueueB = (from t in chosenAmmo
			select new LocalTargetInfo(t)).ToList<LocalTargetInfo>();
			job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
			job.count = Math.Min(job.count, comp.MaxAmmoNeeded(true));
			return job;
		}

		// Token: 0x04003316 RID: 13078
		private const bool forceReloadWhenLookingForWork = false;
	}
}
