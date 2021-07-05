using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070A RID: 1802
	public class JobDriver_CarryToBiosculpterPod : JobDriver
	{
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x00121CCC File Offset: 0x0011FECC
		private Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x00121CF4 File Offset: 0x0011FEF4
		private CompBiosculpterPod Pod
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompBiosculpterPod>();
			}
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x00121D1C File Offset: 0x0011FF1C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Pod.parent, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x00121D72 File Offset: 0x0011FF72
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.Pod.CanAccept(this.Takee));
			Toil goToTakee = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.Takee.IsColonist && !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil startCarryingTakee = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil goToThing = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
			yield return Toils_Jump.JumpIf(goToThing, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return goToTakee;
			yield return startCarryingTakee;
			yield return goToThing;
			yield return JobDriver_EnterBiosculpterPod.PrepareToEnterToil(TargetIndex.B);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.Pod.TryAcceptPawn(this.Takee);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x04001DAC RID: 7596
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04001DAD RID: 7597
		private const TargetIndex PodInd = TargetIndex.B;
	}
}
