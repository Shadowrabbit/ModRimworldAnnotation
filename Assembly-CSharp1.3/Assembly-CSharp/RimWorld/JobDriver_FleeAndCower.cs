using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000715 RID: 1813
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		// Token: 0x0600324F RID: 12879 RVA: 0x001225F8 File Offset: 0x001207F8
		public override string GetReport()
		{
			if (this.pawn.CurJob != this.job || this.pawn.Position != this.job.GetTarget(TargetIndex.A).Cell)
			{
				return base.GetReport();
			}
			return "ReportCowering".Translate();
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x00122654 File Offset: 0x00120854
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 1200,
				tickAction = delegate()
				{
					if (this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
					{
						base.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x04001DBF RID: 7615
		private const int CowerTicks = 1200;

		// Token: 0x04001DC0 RID: 7616
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
