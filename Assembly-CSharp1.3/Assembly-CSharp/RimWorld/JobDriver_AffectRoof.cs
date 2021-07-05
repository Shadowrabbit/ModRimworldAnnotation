using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D8 RID: 1752
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x0011EBFC File Offset: 0x0011CDFC
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060030DF RID: 12511
		protected abstract PathEndMode PathEndMode { get; }

		// Token: 0x060030E0 RID: 12512
		protected abstract void DoEffect();

		// Token: 0x060030E1 RID: 12513
		protected abstract bool DoWorkFailOn();

		// Token: 0x060030E2 RID: 12514 RVA: 0x0011EC1D File Offset: 0x0011CE1D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x0011EC3B File Offset: 0x0011CE3B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Cell, this.job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed);
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x0011EC61 File Offset: 0x0011CE61
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
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A, null);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / 65f, false, -0.5f, false);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x04001D5A RID: 7514
		private float workLeft;

		// Token: 0x04001D5B RID: 7515
		private const TargetIndex CellInd = TargetIndex.A;

		// Token: 0x04001D5C RID: 7516
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		// Token: 0x04001D5D RID: 7517
		private const float BaseWorkAmount = 65f;
	}
}
