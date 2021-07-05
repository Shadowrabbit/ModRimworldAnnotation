using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000704 RID: 1796
	public class JobDriver_VisitSickPawn : JobDriver
	{
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x001215F0 File Offset: 0x0011F7F0
		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x00121618 File Offset: 0x0011F818
		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x0012163C File Offset: 0x0011F83C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Patient, this.job, 1, -1, null, errorOnFailed) && (this.Chair == null || this.pawn.Reserve(this.Chair, this.job, 1, -1, null, errorOnFailed));
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0012169A File Offset: 0x0011F89A
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
					Need_Joy joy = this.Patient.needs.joy;
					if (joy != null)
					{
						joy.GainJoy(this.job.def.joyGainRate * 0.000144f, this.job.def.joyKind);
					}
					if (this.pawn.IsHashIntervalTick(320))
					{
						InteractionDef intDef = (Rand.Value < 0.8f) ? InteractionDefOf.Chitchat : InteractionDefOf.DeepTalk;
						this.pawn.interactions.TryInteractWith(this.Patient, intDef);
					}
					this.pawn.rotationTracker.FaceCell(this.Patient.Position);
					this.pawn.GainComfortFromCellIfPossible(false);
					if (JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null))
					{
						return;
					}
					Need_Joy joy2 = this.pawn.needs.joy;
					if (joy2 != null && joy2.CurLevelPercentage > 0.9999f)
					{
						Need_Joy joy3 = this.Patient.needs.joy;
						if (joy3 != null && joy3.CurLevelPercentage > 0.9999f)
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
					}
				},
				handlingFacing = true,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = this.job.def.joyDuration
			};
			yield break;
		}

		// Token: 0x04001D9E RID: 7582
		private const TargetIndex PatientInd = TargetIndex.A;

		// Token: 0x04001D9F RID: 7583
		private const TargetIndex ChairInd = TargetIndex.B;
	}
}
