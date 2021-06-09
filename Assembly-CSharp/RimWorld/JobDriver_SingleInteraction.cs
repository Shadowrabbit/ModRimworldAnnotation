using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C00 RID: 3072
	public class JobDriver_SingleInteraction : JobDriver
	{
		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004846 RID: 18502 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x000346A1 File Offset: 0x000328A1
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

		// Token: 0x04003044 RID: 12356
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
