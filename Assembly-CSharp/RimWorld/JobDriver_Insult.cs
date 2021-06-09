using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD1 RID: 3025
	public class JobDriver_Insult : JobDriver
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004722 RID: 18210 RVA: 0x00033D89 File Offset: 0x00031F89
		private Pawn Target
		{
			get
			{
				return (Pawn)((Thing)this.pawn.CurJob.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x00033DA6 File Offset: 0x00031FA6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.InsultingSpreeDelayToil();
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield return this.InteractToil();
			yield break;
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x00033DB6 File Offset: 0x00031FB6
		private Toil InteractToil()
		{
			return Toils_General.Do(delegate
			{
				if (this.pawn.interactions.TryInteractWith(this.Target, InteractionDefOf.Insult))
				{
					MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
					if (mentalState_InsultingSpree != null)
					{
						mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
						if (mentalState_InsultingSpree.target == this.Target)
						{
							mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
						}
					}
				}
			});
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x001972B0 File Offset: 0x001954B0
		private Toil InsultingSpreeDelayToil()
		{
			Action action = delegate()
			{
				MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
				if (mentalState_InsultingSpree == null || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			return new Toil
			{
				initAction = action,
				tickAction = action,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}

		// Token: 0x04002FB0 RID: 12208
		private const TargetIndex TargetInd = TargetIndex.A;
	}
}
