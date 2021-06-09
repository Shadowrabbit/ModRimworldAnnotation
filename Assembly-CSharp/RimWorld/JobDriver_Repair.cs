using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B67 RID: 2919
	public class JobDriver_Repair : JobDriver
	{
		// Token: 0x060044B0 RID: 17584 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x00032B25 File Offset: 0x00030D25
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
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			repair.activeSkill = (() => SkillDefOf.Construction);
			yield return repair;
			yield break;
		}

		// Token: 0x04002E8F RID: 11919
		protected float ticksToNextRepair;

		// Token: 0x04002E90 RID: 11920
		private const float WarmupTicks = 80f;

		// Token: 0x04002E91 RID: 11921
		private const float TicksBetweenRepairs = 20f;
	}
}
