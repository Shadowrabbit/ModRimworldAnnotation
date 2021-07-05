using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006BE RID: 1726
	public class JobDriver_InstallRelic : JobDriver
	{
		// Token: 0x0600301A RID: 12314 RVA: 0x0011CF30 File Offset: 0x0011B130
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x0011CF83 File Offset: 0x0011B183
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Relic"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			Toil toil = Toils_General.Wait(300, TargetIndex.B).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			toil.handlingFacing = true;
			yield return toil;
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A, delegate
			{
				this.job.GetTarget(TargetIndex.A).Thing.def.soundDrop.PlayOneShot(new TargetInfo(this.job.GetTarget(TargetIndex.B).Cell, this.pawn.Map, false));
				SoundDefOf.Relic_Installed.PlayOneShot(new TargetInfo(this.job.GetTarget(TargetIndex.B).Cell, this.pawn.Map, false));
			});
			yield break;
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x0011CF94 File Offset: 0x0011B194
		[CompilerGenerated]
		private bool <MakeNewToils>g__ReliquaryFull|5_0()
		{
			CompRelicContainer compRelicContainer = this.pawn.jobs.curJob.GetTarget(TargetIndex.B).Thing.TryGetComp<CompRelicContainer>();
			return compRelicContainer == null || compRelicContainer.Full;
		}

		// Token: 0x04001D2D RID: 7469
		private const TargetIndex RelicInd = TargetIndex.A;

		// Token: 0x04001D2E RID: 7470
		private const TargetIndex ContainerInd = TargetIndex.B;

		// Token: 0x04001D2F RID: 7471
		private const TargetIndex ContainerInteractionCellInd = TargetIndex.C;

		// Token: 0x04001D30 RID: 7472
		private const int InstallTicks = 300;
	}
}
