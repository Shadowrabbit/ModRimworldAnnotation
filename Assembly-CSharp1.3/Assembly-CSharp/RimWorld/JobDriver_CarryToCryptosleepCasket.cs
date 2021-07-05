using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070B RID: 1803
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x00121DE0 File Offset: 0x0011FFE0
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x00121E08 File Offset: 0x00120008
		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x00121E30 File Offset: 0x00120030
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.DropPod, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x00121E81 File Offset: 0x00120081
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.DropPod.Accepts(this.Takee));
			Toil goToTakee = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.DropPod.GetDirectlyHeldThings().Count > 0).FailOn(() => !this.Takee.Downed).FailOn(() => !this.pawn.CanReach(this.Takee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil startCarryingTakee = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil goToThing = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
			yield return Toils_Jump.JumpIf(goToThing, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return goToTakee;
			yield return startCarryingTakee;
			yield return goToThing;
			Toil toil = Toils_General.Wait(500, TargetIndex.B);
			toil.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return toil;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.DropPod.TryAcceptThing(this.Takee, true);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x00121E91 File Offset: 0x00120091
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				this.Takee
			};
		}

		// Token: 0x04001DAE RID: 7598
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04001DAF RID: 7599
		private const TargetIndex DropPodInd = TargetIndex.B;
	}
}
