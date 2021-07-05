using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000824 RID: 2084
	public class JobDriver_MergeIntoGaumakerPod : JobDriver
	{
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x00138850 File Offset: 0x00136A50
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x001389B0 File Offset: 0x00136BB0
		private CompGaumakerPod GaumakerPod
		{
			get
			{
				return this.job.targetB.Thing.TryGetComp<CompGaumakerPod>();
			}
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x001389C7 File Offset: 0x00136BC7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnDespawnedOrNull(TargetIndex.B);
			this.FailOn(() => this.GaumakerPod.Full);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.B, 120, true, false);
			yield return Toils_General.Do(delegate
			{
				this.GaumakerPod.TryAcceptPawn(this.pawn);
			});
			yield break;
		}

		// Token: 0x04001F12 RID: 7954
		private const int WaitTicks = 120;
	}
}
