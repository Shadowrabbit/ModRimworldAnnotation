using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA7 RID: 2983
	public class JobDriver_WatchBuilding : JobDriver
	{
		// Token: 0x06004616 RID: 17942 RVA: 0x001943B4 File Offset: 0x001925B4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed))
			{
				return false;
			}
			if (!this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (base.TargetC.HasThing)
			{
				if (base.TargetC.Thing is Building_Bed)
				{
					if (!this.pawn.Reserve(this.job.targetC, this.job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null, errorOnFailed))
					{
						return false;
					}
				}
				else if (!this.pawn.Reserve(this.job.targetC, this.job, 1, -1, null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x001944A0 File Offset: 0x001926A0
		public override bool CanBeginNowWhileLyingDown()
		{
			return base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, base.TargetC);
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x00033512 File Offset: 0x00031712
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
				CS$<>8__locals1.watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true);
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
				CS$<>8__locals1.watch.WithEffect(() => CS$<>8__locals1.<>4__this.TargetA.Thing.def.building.effectWatching, new Func<LocalTargetInfo>(CS$<>8__locals1.<MakeNewToils>g__EffectTargetGetter|3));
			}
			yield return CS$<>8__locals1.watch;
			yield break;
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x001944E8 File Offset: 0x001926E8
		protected virtual void WatchTickAction()
		{
			this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x0019322C File Offset: 0x0019142C
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
