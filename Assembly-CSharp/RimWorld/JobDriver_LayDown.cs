using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C4F RID: 3151
	public class JobDriver_LayDown : JobDriver
	{
		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x0019F210 File Offset: 0x0019D410
		public Building_Bed Bed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0019F238 File Offset: 0x0019D438
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return !this.job.GetTarget(TargetIndex.A).HasThing || this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x0003535C File Offset: 0x0003355C
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x00035375 File Offset: 0x00033575
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.job.GetTarget(TargetIndex.A).HasThing;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			}
			yield return Toils_LayDown.LayDown(TargetIndex.A, hasBed, true, true, true);
			yield break;
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x00035385 File Offset: 0x00033585
		public override string GetReport()
		{
			if (this.asleep)
			{
				return "ReportSleeping".Translate();
			}
			return "ReportResting".Translate();
		}

		// Token: 0x0400311B RID: 12571
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
	}
}
