using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C1 RID: 1729
	public class JobDriver_Nuzzle : JobDriver
	{
		// Token: 0x0600302E RID: 12334 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x0011D1F0 File Offset: 0x0011B3F0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).socialMode = RandomSocialMode.Off;
			Toils_General.WaitWith(TargetIndex.A, 100, false, true).socialMode = RandomSocialMode.Off;
			yield return Toils_General.Do(delegate
			{
				Pawn recipient = (Pawn)this.pawn.CurJob.targetA.Thing;
				this.pawn.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
			});
			yield break;
		}

		// Token: 0x04001D36 RID: 7478
		private const int NuzzleDuration = 100;
	}
}
