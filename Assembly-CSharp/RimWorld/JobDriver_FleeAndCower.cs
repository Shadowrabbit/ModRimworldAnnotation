using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC3 RID: 3011
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		// Token: 0x060046C4 RID: 18116 RVA: 0x00196130 File Offset: 0x00194330
		public override string GetReport()
		{
			if (this.pawn.CurJob != this.job || this.pawn.Position != this.job.GetTarget(TargetIndex.A).Cell)
			{
				return base.GetReport();
			}
			return "ReportCowering".Translate();
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x00033A04 File Offset: 0x00031C04
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

		// Token: 0x04002F84 RID: 12164
		private const int CowerTicks = 1200;

		// Token: 0x04002F85 RID: 12165
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
