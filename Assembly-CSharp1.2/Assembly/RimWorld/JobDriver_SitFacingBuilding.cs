using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9B RID: 2971
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		// Token: 0x060045C1 RID: 17857 RVA: 0x0019383C File Offset: 0x00191A3C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed) && this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x00033316 File Offset: 0x00031516
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

		// Token: 0x060045C3 RID: 17859 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ModifyPlayToil(Toil toil)
		{
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x0019322C File Offset: 0x0019142C
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
