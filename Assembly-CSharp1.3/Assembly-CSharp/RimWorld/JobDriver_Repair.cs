using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E0 RID: 1760
	public class JobDriver_Repair : JobDriver
	{
		// Token: 0x0600311A RID: 12570 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x0011F1D4 File Offset: 0x0011D3D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil repair = new Toil();
			repair.initAction = delegate()
			{
				this.ticksToNextRepair = 80f;
			};
			repair.tickAction = delegate()
			{
				Pawn actor = repair.actor;
				actor.skills.Learn(SkillDefOf.Construction, 0.05f, false);
				actor.rotationTracker.FaceTarget(actor.CurJob.GetTarget(TargetIndex.A));
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				this.ticksToNextRepair -= num;
				if (this.ticksToNextRepair <= 0f)
				{
					this.ticksToNextRepair += 20f;
					this.TargetThingA.HitPoints++;
					this.TargetThingA.HitPoints = Mathf.Min(this.TargetThingA.HitPoints, this.TargetThingA.MaxHitPoints);
					this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)this.TargetThingA);
					if (this.TargetThingA.HitPoints == this.TargetThingA.MaxHitPoints)
					{
						actor.records.Increment(RecordDefOf.ThingsRepaired);
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					}
				}
			};
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A, null);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			repair.activeSkill = (() => SkillDefOf.Construction);
			repair.handlingFacing = true;
			yield return repair;
			yield break;
		}

		// Token: 0x04001D68 RID: 7528
		protected float ticksToNextRepair;

		// Token: 0x04001D69 RID: 7529
		private const float WarmupTicks = 80f;

		// Token: 0x04001D6A RID: 7530
		private const float TicksBetweenRepairs = 20f;
	}
}
