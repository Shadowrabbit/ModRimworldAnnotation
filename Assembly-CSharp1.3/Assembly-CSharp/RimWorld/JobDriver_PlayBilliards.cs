using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006F8 RID: 1784
	public class JobDriver_PlayBilliards : JobDriver
	{
		// Token: 0x060031A3 RID: 12707 RVA: 0x00120C0F File Offset: 0x0011EE0F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed);
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00120C40 File Offset: 0x0011EE40
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			Toil chooseCell = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
			yield return chooseCell;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Walk;
			};
			toil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				if (this.ticksLeftThisToil == 300)
				{
					SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
			};
			toil.handlingFacing = true;
			toil.socialMode = RandomSocialMode.SuperActive;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 600;
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return toil;
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return Toils_Jump.Jump(chooseCell);
			yield break;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x00120C50 File Offset: 0x0011EE50
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		// Token: 0x04001D95 RID: 7573
		private const int ShotDuration = 600;
	}
}
