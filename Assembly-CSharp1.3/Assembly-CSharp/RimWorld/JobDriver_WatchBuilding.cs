using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000705 RID: 1797
	public class JobDriver_WatchBuilding : JobDriver
	{
		// Token: 0x060031EB RID: 12779 RVA: 0x001217EC File Offset: 0x0011F9EC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed) && this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed) && (!base.TargetC.HasThing || !(base.TargetC.Thing is Building_Bed) || this.pawn.Reserve(this.job.targetC, this.job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null, errorOnFailed));
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x001218B4 File Offset: 0x0011FAB4
		public override bool CanBeginNowWhileLyingDown()
		{
			return base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, base.TargetC);
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x001218F9 File Offset: 0x0011FAF9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_WatchBuilding.<>c__DisplayClass2_0 CS$<>8__locals1 = new JobDriver_WatchBuilding.<>c__DisplayClass2_0();
			CS$<>8__locals1.<>4__this = this;
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed)
			{
				this.KeepLyingDown(TargetIndex.C);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.C);
				CS$<>8__locals1.watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true, PawnPosture.LayingOnGroundNormal);
				CS$<>8__locals1.watch.AddFailCondition(() => !CS$<>8__locals1.watch.actor.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				CS$<>8__locals1.watch = new Toil();
			}
			CS$<>8__locals1.watch.AddPreTickAction(delegate
			{
				CS$<>8__locals1.<>4__this.WatchTickAction();
			});
			CS$<>8__locals1.watch.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(CS$<>8__locals1.<>4__this.pawn);
			});
			CS$<>8__locals1.watch.defaultCompleteMode = ToilCompleteMode.Delay;
			CS$<>8__locals1.watch.defaultDuration = this.job.def.joyDuration;
			CS$<>8__locals1.watch.handlingFacing = true;
			if (base.TargetA.Thing.def.building != null && base.TargetA.Thing.def.building.effectWatching != null)
			{
				CS$<>8__locals1.watch.WithEffect(() => CS$<>8__locals1.<>4__this.TargetA.Thing.def.building.effectWatching, new Func<LocalTargetInfo>(CS$<>8__locals1.<MakeNewToils>g__EffectTargetGetter|3), null);
			}
			yield return CS$<>8__locals1.watch;
			yield break;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0012190C File Offset: 0x0011FB0C
		protected virtual void WatchTickAction()
		{
			this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x00121960 File Offset: 0x0011FB60
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
