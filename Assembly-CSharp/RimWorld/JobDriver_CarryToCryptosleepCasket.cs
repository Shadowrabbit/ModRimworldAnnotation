using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB0 RID: 2992
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x0016B214 File Offset: 0x00169414
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x00195174 File Offset: 0x00193374
		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x0019519C File Offset: 0x0019339C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.DropPod, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x0003367B File Offset: 0x0003187B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.DropPod.Accepts(this.Takee));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.DropPod.GetDirectlyHeldThings().Count > 0).FailOn(() => !this.Takee.Downed).FailOn(() => !this.pawn.CanReach(this.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
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

		// Token: 0x06004656 RID: 18006 RVA: 0x0003368B File Offset: 0x0003188B
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				this.Takee
			};
		}

		// Token: 0x04002F4C RID: 12108
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04002F4D RID: 12109
		private const TargetIndex DropPodInd = TargetIndex.B;
	}
}
