using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E8 RID: 1768
	public class JobDriver_DeliverPawnToAltar : JobDriver
	{
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x0011F820 File Offset: 0x0011DA20
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x0011F848 File Offset: 0x0011DA48
		protected Building DropAltar
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x0011F870 File Offset: 0x0011DA70
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			base.Map.reservationManager.ReleaseAllForTarget(this.Takee);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.DropAltar, this.job, 1, 0, null, errorOnFailed);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x0011F8D7 File Offset: 0x0011DAD7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Deliver to altar job"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.C);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOn(() => !this.pawn.CanReach(this.DropAltar, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return toil;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil goToAltar = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return Toils_Jump.JumpIf(goToAltar, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return startCarrying;
			yield return goToAltar;
			IntVec3 position = default(IntVec3);
			if (this.DropAltar.def.hasInteractionCell)
			{
				IntVec3 interactionCell = this.DropAltar.InteractionCell;
				IntVec3 b = (this.DropAltar.Position - interactionCell).ClampInsideRect(new CellRect(-1, -1, 3, 3));
				position = interactionCell + b;
			}
			else if (this.DropAltar.def.Size.z % 2 != 0)
			{
				position = this.DropAltar.Position + new IntVec3(0, 0, -this.DropAltar.def.Size.z / 2).RotatedBy(this.DropAltar.Rotation);
			}
			yield return Toils_General.Do(delegate
			{
				this.job.SetTarget(TargetIndex.B, position);
			});
			yield return Toils_Reserve.Release(TargetIndex.C);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
			yield break;
		}

		// Token: 0x04001D6F RID: 7535
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x04001D70 RID: 7536
		private const TargetIndex TargetCellIndex = TargetIndex.B;

		// Token: 0x04001D71 RID: 7537
		private const TargetIndex AltarIndex = TargetIndex.C;
	}
}
