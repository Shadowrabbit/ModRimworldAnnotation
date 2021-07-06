using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CC2 RID: 3266
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		// Token: 0x06004B9C RID: 19356 RVA: 0x00035E18 File Offset: 0x00034018
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x001A634C File Offset: 0x001A454C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Thing t) => t.def.hasInteractionCell && t.def.HasComp(typeof(CompMannable)) && pawn.CanReserve(t, 1, -1, null, false) && JobDriver_ManTurret.FindAmmoForTurret(pawn, (Building_TurretGun)t) != null;
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				return job;
			}
			return null;
		}

		// Token: 0x06004B9E RID: 19358
		protected abstract IntVec3 GetRoot(Pawn pawn);

		// Token: 0x040031E7 RID: 12775
		public float maxDistFromPoint = -1f;
	}
}
