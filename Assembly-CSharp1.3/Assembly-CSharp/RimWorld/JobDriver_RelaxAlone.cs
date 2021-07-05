using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006FD RID: 1789
	public class JobDriver_RelaxAlone : JobDriver
	{
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060031B4 RID: 12724 RVA: 0x00120F04 File Offset: 0x0011F104
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing is Building_Bed;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060031B5 RID: 12725 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x00120F2D File Offset: 0x0011F12D
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x00120F50 File Offset: 0x0011F150
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

		// Token: 0x060031B8 RID: 12728 RVA: 0x00120FCD File Offset: 0x0011F1CD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.A);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
				toil = Toils_LayDown.LayDown(TargetIndex.A, true, false, this.CanSleep, true, PawnPosture.LayingOnGroundNormal);
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

		// Token: 0x060031B9 RID: 12729 RVA: 0x00120FE0 File Offset: 0x0011F1E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}

		// Token: 0x04001D98 RID: 7576
		protected Rot4 faceDir = Rot4.Invalid;

		// Token: 0x04001D99 RID: 7577
		protected const TargetIndex SpotOrBedInd = TargetIndex.A;
	}
}
