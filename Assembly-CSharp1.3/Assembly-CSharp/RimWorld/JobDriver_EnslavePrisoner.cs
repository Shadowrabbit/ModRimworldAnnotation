using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074B RID: 1867
	public class JobDriver_EnslavePrisoner : JobDriver
	{
		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x00125A6B File Offset: 0x00123C6B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, this.Prisoner.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ReduceWill(this.pawn, this.Prisoner);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, this.Prisoner.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ReduceWill(this.pawn, this.Prisoner);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, this.Prisoner.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !this.Prisoner.guest.ScheduledForInteraction);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryEnslave(TargetIndex.A);
			yield break;
		}
	}
}
