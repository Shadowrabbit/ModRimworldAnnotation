using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074A RID: 1866
	public class JobDriver_ConvertPrisoner : JobDriver
	{
		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x00125A22 File Offset: 0x00123C22
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure);
			int num;
			for (int i = 0; i < 3; i = num + 1)
			{
				yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, this.Prisoner.guest.interactionMode);
				yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
				yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Prisoner, InteractionDefOf.Chitchat);
				num = i;
			}
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, this.Prisoner.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !this.Prisoner.guest.ScheduledForInteraction);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryConvert(TargetIndex.A);
			yield break;
		}

		// Token: 0x04001E14 RID: 7700
		private const int NumTalks = 3;
	}
}
