using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071E RID: 1822
	public class JobDriver_Insult : JobDriver
	{
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x0012312A File Offset: 0x0012132A
		private Pawn Target
		{
			get
			{
				return (Pawn)((Thing)this.pawn.CurJob.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x00123147 File Offset: 0x00121347
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

		// Token: 0x0600329C RID: 12956 RVA: 0x00123157 File Offset: 0x00121357
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

		// Token: 0x0600329D RID: 12957 RVA: 0x0012316C File Offset: 0x0012136C
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

		// Token: 0x04001DCD RID: 7629
		private const TargetIndex TargetInd = TargetIndex.A;
	}
}
