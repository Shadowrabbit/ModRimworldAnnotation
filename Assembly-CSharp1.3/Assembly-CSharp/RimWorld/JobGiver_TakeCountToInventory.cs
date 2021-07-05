using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D3 RID: 2003
	public class JobGiver_TakeCountToInventory : ThinkNode_JobGiver
	{
		// Token: 0x060035DE RID: 13790 RVA: 0x001311F0 File Offset: 0x0012F3F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			int toTake = Math.Max(this.count - pawn.inventory.Count(this.def), 0);
			if (toTake == 0)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(this.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => x.stackCount >= toTake && !x.IsForbidden(pawn) && pawn.CanReserve(x, 10, toTake, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
				job.count = toTake;
				return job;
			}
			return null;
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001312AE File Offset: 0x0012F4AE
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCountToInventory jobGiver_TakeCountToInventory = (JobGiver_TakeCountToInventory)base.DeepCopy(resolve);
			jobGiver_TakeCountToInventory.def = this.def;
			jobGiver_TakeCountToInventory.count = this.count;
			return jobGiver_TakeCountToInventory;
		}

		// Token: 0x04001EC4 RID: 7876
		public ThingDef def;

		// Token: 0x04001EC5 RID: 7877
		public int count = 1;
	}
}
