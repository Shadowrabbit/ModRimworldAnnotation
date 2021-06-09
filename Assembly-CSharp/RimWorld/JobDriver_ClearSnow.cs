using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB5 RID: 2997
	public class JobDriver_ClearSnow : JobDriver
	{
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x000337B5 File Offset: 0x000319B5
		private float TotalNeededWork
		{
			get
			{
				return 50f * base.Map.snowGrid.GetDepth(base.TargetLocA);
			}
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x000337D3 File Offset: 0x000319D3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil clearToil = new Toil();
			clearToil.tickAction = delegate()
			{
				float statValue = clearToil.actor.GetStatValue(StatDefOf.GeneralLaborSpeed, true);
				this.workDone += statValue;
				if (this.workDone >= this.TotalNeededWork)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
					this.ReadyForNextToil();
					return;
				}
			};
			clearToil.defaultCompleteMode = ToilCompleteMode.Never;
			clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A);
			clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
			clearToil.WithProgressBar(TargetIndex.A, () => this.workDone / this.TotalNeededWork, true, -0.5f);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
			yield break;
		}

		// Token: 0x04002F5E RID: 12126
		private float workDone;

		// Token: 0x04002F5F RID: 12127
		private const float ClearWorkPerSnowDepth = 50f;
	}
}
