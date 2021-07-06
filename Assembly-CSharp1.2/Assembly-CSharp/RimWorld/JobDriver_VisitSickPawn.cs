using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA5 RID: 2981
	public class JobDriver_VisitSickPawn : JobDriver
	{
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004607 RID: 17927 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004608 RID: 17928 RVA: 0x00190280 File Offset: 0x0018E480
		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x001940D4 File Offset: 0x001922D4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Patient, this.job, 1, -1, null, errorOnFailed) && (this.Chair == null || this.pawn.Reserve(this.Chair, this.job, 1, -1, null, errorOnFailed));
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x000334B9 File Offset: 0x000316B9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.Patient.InBed() || !this.Patient.Awake());
			if (this.Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			if (this.Chair != null)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			}
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return new Toil
			{
				tickAction = delegate()
				{
					this.Patient.needs.joy.GainJoy(this.job.def.joyGainRate * 0.000144f, this.job.def.joyKind);
					if (this.pawn.IsHashIntervalTick(320))
					{
						InteractionDef intDef = (Rand.Value < 0.8f) ? InteractionDefOf.Chitchat : InteractionDefOf.DeepTalk;
						this.pawn.interactions.TryInteractWith(this.Patient, intDef);
					}
					this.pawn.rotationTracker.FaceCell(this.Patient.Position);
					this.pawn.GainComfortFromCellIfPossible(false);
					JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
					if (this.pawn.needs.joy.CurLevelPercentage > 0.9999f && this.Patient.needs.joy.CurLevelPercentage > 0.9999f)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					}
				},
				handlingFacing = true,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = this.job.def.joyDuration
			};
			yield break;
		}

		// Token: 0x04002F29 RID: 12073
		private const TargetIndex PatientInd = TargetIndex.A;

		// Token: 0x04002F2A RID: 12074
		private const TargetIndex ChairInd = TargetIndex.B;
	}
}
