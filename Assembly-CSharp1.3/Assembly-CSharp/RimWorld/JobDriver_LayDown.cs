using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public class JobDriver_LayDown : JobDriver
	{
		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x001232EC File Offset: 0x001214EC
		public Building_Bed Bed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_Bed;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool LookForOtherJobs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x00123312 File Offset: 0x00121512
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.Bed == null || this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x0012334E File Offset: 0x0012154E
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00123367 File Offset: 0x00121567
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.Bed != null;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			}
			yield return this.LayDownToil(hasBed);
			yield break;
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x00123377 File Offset: 0x00121577
		public virtual Toil LayDownToil(bool hasBed)
		{
			return Toils_LayDown.LayDown(TargetIndex.A, hasBed, this.LookForOtherJobs, this.CanSleep, this.CanRest, PawnPosture.LayingOnGroundNormal);
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x00123393 File Offset: 0x00121593
		public override string GetReport()
		{
			if (this.asleep)
			{
				return "ReportSleeping".Translate();
			}
			return "ReportResting".Translate();
		}

		// Token: 0x04001DCE RID: 7630
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
	}
}
