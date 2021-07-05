using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000749 RID: 1865
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Talkee
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x0012599E File Offset: 0x00123B9E
		protected override string ReportStringProcessed(string str)
		{
			if (this.Talkee.guest.interactionMode == PrisonerInteractionModeDefOf.ReduceResistance)
			{
				return "JobReport_ReduceResistance".Translate(this.Talkee);
			}
			return base.ReportStringProcessed(str);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x001259D9 File Offset: 0x00123BD9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Talkee.IsPrisonerOfColony || !this.Talkee.guest.PrisonerIsSecure);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee, null);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee, null);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee, null);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee, null);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee, null);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !this.Talkee.guest.ScheduledForInteraction);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			yield break;
		}
	}
}
