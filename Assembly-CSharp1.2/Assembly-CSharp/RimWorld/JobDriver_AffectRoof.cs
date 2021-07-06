using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B50 RID: 2896
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x0018F88C File Offset: 0x0018DA8C
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x0600441B RID: 17435
		protected abstract PathEndMode PathEndMode { get; }

		// Token: 0x0600441C RID: 17436
		protected abstract void DoEffect();

		// Token: 0x0600441D RID: 17437
		protected abstract bool DoWorkFailOn();

		// Token: 0x0600441E RID: 17438 RVA: 0x0003262E File Offset: 0x0003082E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0003264C File Offset: 0x0003084C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Cell, this.job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed);
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x00032672 File Offset: 0x00030872
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = 65f;
			};
			doWork.tickAction = delegate()
			{
				float num = doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				this.workLeft -= num;
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
			doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
			doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / 65f, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x04002E4E RID: 11854
		private float workLeft;

		// Token: 0x04002E4F RID: 11855
		private const TargetIndex CellInd = TargetIndex.A;

		// Token: 0x04002E50 RID: 11856
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		// Token: 0x04002E51 RID: 11857
		private const float BaseWorkAmount = 65f;
	}
}
