using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2B RID: 2859
	public class JobDriver_Nuzzle : JobDriver
	{
		// Token: 0x0600431B RID: 17179 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00031D58 File Offset: 0x0002FF58
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

		// Token: 0x04002DE3 RID: 11747
		private const int NuzzleDuration = 100;
	}
}
