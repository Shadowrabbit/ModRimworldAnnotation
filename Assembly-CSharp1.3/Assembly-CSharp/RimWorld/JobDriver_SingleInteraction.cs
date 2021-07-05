using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000732 RID: 1842
	public class JobDriver_SingleInteraction : JobDriver
	{
		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x0600331C RID: 13084 RVA: 0x0012444C File Offset: 0x0012264C
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x00124472 File Offset: 0x00122672
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield return Toils_Interpersonal.Interact(TargetIndex.A, this.job.interaction);
			yield break;
		}

		// Token: 0x04001DF3 RID: 7667
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
