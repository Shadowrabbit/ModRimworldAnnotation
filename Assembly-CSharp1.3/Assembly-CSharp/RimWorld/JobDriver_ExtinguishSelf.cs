using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E5 RID: 1765
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x0011F3FD File Offset: 0x0011D5FD
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x0011F494 File Offset: 0x0011D694
		public override string GetReport()
		{
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap, this.TargetFire.parent.Named("TARGET"));
			}
			return "ReportExtinguishingFire".Translate();
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x0011F4FF File Offset: 0x0011D6FF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 150
			};
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.TargetFire.Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			};
			toil.FailOnDestroyedOrNull(TargetIndex.A);
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}
	}
}
