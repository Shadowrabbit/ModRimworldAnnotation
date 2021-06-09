using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B99 RID: 2969
	public class JobDriver_RelaxAlone : JobDriver
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060045AF RID: 17839 RVA: 0x00193588 File Offset: 0x00191788
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing is Building_Bed;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00033260 File Offset: 0x00031460
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x001935B4 File Offset: 0x001917B4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.FromBed)
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, ((Building_Bed)this.job.GetTarget(TargetIndex.A).Thing).SleepingSlotsCount, 0, null, errorOnFailed))
				{
					return false;
				}
			}
			else if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x00033283 File Offset: 0x00031483
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.A);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
				toil = Toils_LayDown.LayDown(TargetIndex.A, true, false, this.CanSleep, true);
				toil.AddFailCondition(() => !this.pawn.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil = new Toil();
				toil.initAction = delegate()
				{
					this.faceDir = (this.job.def.faceDir.IsValid ? this.job.def.faceDir : Rot4.Random);
				};
				toil.handlingFacing = true;
			}
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.AddPreTickAction(delegate
			{
				if (this.faceDir.IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir.FacingCell);
				}
				this.pawn.GainComfortFromCellIfPossible(false);
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			});
			yield return toil;
			yield break;
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00193634 File Offset: 0x00191834
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}

		// Token: 0x04002F0F RID: 12047
		protected Rot4 faceDir = Rot4.Invalid;

		// Token: 0x04002F10 RID: 12048
		protected const TargetIndex SpotOrBedInd = TargetIndex.A;
	}
}
