using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006FE RID: 1790
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		// Token: 0x060031BE RID: 12734 RVA: 0x001210CC File Offset: 0x0011F2CC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed) && this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed);
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x0012112E File Offset: 0x0011F32E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = new Toil();
			toil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
				this.pawn.GainComfortFromCellIfPossible(false);
				Pawn pawn = this.pawn;
				Building joySource = (Building)base.TargetThingA;
				JoyUtility.JoyTickCheckEnd(pawn, this.job.doUntilGatheringEnded ? JoyTickFullJoyAction.None : JoyTickFullJoyAction.EndJob, 1f, joySource);
			};
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = (this.job.doUntilGatheringEnded ? this.job.expiryInterval : this.job.def.joyDuration);
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			this.ModifyPlayToil(toil);
			yield return toil;
			yield break;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ModifyPlayToil(Toil toil)
		{
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00121140 File Offset: 0x0011F340
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}
	}
}
