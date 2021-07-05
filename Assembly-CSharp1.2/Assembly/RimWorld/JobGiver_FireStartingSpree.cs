using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D27 RID: 3367
	internal class JobGiver_FireStartingSpree : ThinkNode_JobGiver
	{
		// Token: 0x06004D24 RID: 19748 RVA: 0x00036A7D File Offset: 0x00034C7D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FireStartingSpree jobGiver_FireStartingSpree = (JobGiver_FireStartingSpree)base.DeepCopy(resolve);
			jobGiver_FireStartingSpree.waitTicks = this.waitTicks;
			return jobGiver_FireStartingSpree;
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x001AD430 File Offset: 0x001AB630
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.nextMoveOrderIsWait)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.waitTicks.RandomInRange;
				pawn.mindState.nextMoveOrderIsWait = false;
				return job;
			}
			if (Rand.Value < 0.75f)
			{
				Thing thing = this.TryFindRandomIgniteTarget(pawn);
				if (thing != null)
				{
					pawn.mindState.nextMoveOrderIsWait = true;
					return JobMaker.MakeJob(JobDefOf.Ignite, thing);
				}
			}
			IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 10f, null, Danger.Deadly);
			if (c.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = true;
				return JobMaker.MakeJob(JobDefOf.GotoWander, c);
			}
			return null;
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x001AD4E4 File Offset: 0x001AB6E4
		private Thing TryFindRandomIgniteTarget(Pawn pawn)
		{
			Region region;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Region candidateRegion) => !candidateRegion.IsForbiddenEntirely(pawn), 100, out region, RegionType.Set_Passable))
			{
				return null;
			}
			JobGiver_FireStartingSpree.potentialTargets.Clear();
			List<Thing> allThings = region.ListerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && thing.FlammableNow && !thing.IsBurning() && !thing.OccupiedRect().Contains(pawn.Position))
				{
					JobGiver_FireStartingSpree.potentialTargets.Add(thing);
				}
			}
			if (JobGiver_FireStartingSpree.potentialTargets.NullOrEmpty<Thing>())
			{
				return null;
			}
			return JobGiver_FireStartingSpree.potentialTargets.RandomElement<Thing>();
		}

		// Token: 0x040032C2 RID: 12994
		private IntRange waitTicks = new IntRange(80, 140);

		// Token: 0x040032C3 RID: 12995
		private const float FireStartChance = 0.75f;

		// Token: 0x040032C4 RID: 12996
		private static List<Thing> potentialTargets = new List<Thing>();
	}
}
