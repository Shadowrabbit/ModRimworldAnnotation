using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070D RID: 1805
	public class JobDriver_ClearSnow : JobDriver
	{
		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x00121FE1 File Offset: 0x001201E1
		private float TotalNeededWork
		{
			get
			{
				return 50f * base.Map.snowGrid.GetDepth(base.TargetLocA);
			}
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x00121FFF File Offset: 0x001201FF
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
			clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A, null);
			clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth, 1f);
			clearToil.WithProgressBar(TargetIndex.A, () => this.workDone / this.TotalNeededWork, true, -0.5f, false);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
			yield break;
		}

		// Token: 0x04001DB4 RID: 7604
		private float workDone;

		// Token: 0x04001DB5 RID: 7605
		private const float ClearWorkPerSnowDepth = 50f;
	}
}
