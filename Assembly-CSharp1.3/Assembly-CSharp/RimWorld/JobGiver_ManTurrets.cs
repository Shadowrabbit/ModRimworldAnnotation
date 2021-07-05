using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007B2 RID: 1970
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		// Token: 0x06003574 RID: 13684 RVA: 0x0012E364 File Offset: 0x0012C564
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x0012E380 File Offset: 0x0012C580
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (!t.def.hasInteractionCell)
				{
					return false;
				}
				if (!t.def.HasComp(typeof(CompMannable)))
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (JobDriver_ManTurret.FindAmmoForTurret(pawn, (Building_TurretGun)t) == null)
				{
					return false;
				}
				CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
				return compRefuelable == null || compRefuelable.IsFull || JobDriver_ManTurret.FindFuelForTurret(pawn, (Building_TurretGun)t) != null;
			};
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				return job;
			}
			return null;
		}

		// Token: 0x06003576 RID: 13686
		protected abstract IntVec3 GetRoot(Pawn pawn);

		// Token: 0x04001E99 RID: 7833
		public float maxDistFromPoint = -1f;
	}
}
