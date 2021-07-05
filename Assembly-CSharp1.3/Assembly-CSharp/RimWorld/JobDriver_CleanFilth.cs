using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070C RID: 1804
	public class JobDriver_CleanFilth : JobDriver
	{
		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x00121F30 File Offset: 0x00120130
		private Filth Filth
		{
			get
			{
				return (Filth)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x00121F56 File Offset: 0x00120156
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x00121F79 File Offset: 0x00120179
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue).JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			Toil clean = new Toil();
			clean.initAction = delegate()
			{
				this.cleaningWorkDone = 0f;
				this.totalCleaningWorkDone = 0f;
				this.totalCleaningWorkRequired = this.Filth.def.filth.cleaningWorkToReduceThickness * (float)this.Filth.thickness;
			};
			clean.tickAction = delegate()
			{
				Filth filth = this.Filth;
				this.cleaningWorkDone += 1f;
				this.totalCleaningWorkDone += 1f;
				if (this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
				{
					filth.ThinFilth();
					this.cleaningWorkDone = 0f;
					if (filth.Destroyed)
					{
						clean.actor.records.Increment(RecordDefOf.MessesCleaned);
						this.ReadyForNextToil();
						return;
					}
				}
			};
			clean.defaultCompleteMode = ToilCompleteMode.Never;
			clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A, null);
			clean.WithProgressBar(TargetIndex.A, () => this.totalCleaningWorkDone / this.totalCleaningWorkRequired, true, -0.5f, false);
			clean.PlaySustainerOrSound(delegate()
			{
				ThingDef def = this.Filth.def;
				if (!def.filth.cleaningSound.NullOrUndefined())
				{
					return def.filth.cleaningSound;
				}
				return SoundDefOf.Interact_CleanFilth;
			}, 1f);
			clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			yield return clean;
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x00121F8C File Offset: 0x0012018C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}

		// Token: 0x04001DB0 RID: 7600
		private float cleaningWorkDone;

		// Token: 0x04001DB1 RID: 7601
		private float totalCleaningWorkDone;

		// Token: 0x04001DB2 RID: 7602
		private float totalCleaningWorkRequired;

		// Token: 0x04001DB3 RID: 7603
		private const TargetIndex FilthInd = TargetIndex.A;
	}
}
