using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B76 RID: 2934
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060044F5 RID: 17653 RVA: 0x00032C65 File Offset: 0x00030E65
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001917E8 File Offset: 0x0018F9E8
		public override string GetReport()
		{
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap, this.TargetFire.parent.Named("TARGET"));
			}
			return "ReportExtinguishingFire".Translate();
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00032CB6 File Offset: 0x00030EB6
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
