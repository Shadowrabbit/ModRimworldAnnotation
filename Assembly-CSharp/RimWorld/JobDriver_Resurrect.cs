using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFE RID: 3070
	public class JobDriver_Resurrect : JobDriver
	{
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06004838 RID: 18488 RVA: 0x00196508 File Offset: 0x00194708
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004839 RID: 18489 RVA: 0x00190280 File Offset: 0x0018E480
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00199DA0 File Offset: 0x00197FA0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x00034667 File Offset: 0x00032867
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(600, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			yield return Toils_General.Do(new Action(this.Resurrect));
			yield break;
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x00199DF4 File Offset: 0x00197FF4
		private void Resurrect()
		{
			Pawn innerPawn = this.Corpse.InnerPawn;
			ResurrectionUtility.ResurrectWithSideEffects(innerPawn);
			Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0400303D RID: 12349
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x0400303E RID: 12350
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x0400303F RID: 12351
		private const int DurationTicks = 600;
	}
}
