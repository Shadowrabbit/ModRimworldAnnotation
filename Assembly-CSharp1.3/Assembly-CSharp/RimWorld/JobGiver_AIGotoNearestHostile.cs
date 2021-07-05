using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000780 RID: 1920
	public class JobGiver_AIGotoNearestHostile : ThinkNode_JobGiver
	{
		// Token: 0x060034D8 RID: 13528 RVA: 0x0012B210 File Offset: 0x00129410
		protected override Job TryGiveJob(Pawn pawn)
		{
			float num = float.MaxValue;
			Thing thing = null;
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget))
				{
					Thing thing2 = (Thing)attackTarget;
					int num2 = thing2.Position.DistanceToSquared(pawn.Position);
					if ((float)num2 < num && pawn.CanReach(thing2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						num = (float)num2;
						thing = thing2;
					}
				}
			}
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, thing);
				job.checkOverrideOnExpire = true;
				job.expiryInterval = 500;
				job.collideWithPawns = true;
				return job;
			}
			return null;
		}
	}
}
