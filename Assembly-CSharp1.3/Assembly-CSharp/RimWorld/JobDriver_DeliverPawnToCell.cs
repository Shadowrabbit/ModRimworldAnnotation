using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006E9 RID: 1769
	public class JobDriver_DeliverPawnToCell : JobDriver
	{
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x0011F8E8 File Offset: 0x0011DAE8
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x0011F90E File Offset: 0x0011DB0E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			base.Map.reservationManager.ReleaseAllForTarget(this.Takee);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x0011F946 File Offset: 0x0011DB46
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => !this.pawn.CanReach(this.job.GetTarget(TargetIndex.B), PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return toil;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Jump.JumpIf(gotoCell, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return startCarrying;
			yield return gotoCell;
			yield return Toils_General.Do(delegate
			{
				if (!this.job.ritualTag.NullOrEmpty())
				{
					Lord lord = this.Takee.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.Takee, this.job.ritualTag);
					}
					Lord lord2 = this.pawn.GetLord();
					lordJob_Ritual = (((lord2 != null) ? lord2.LordJob : null) as LordJob_Ritual);
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
					}
				}
			});
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
			yield break;
		}

		// Token: 0x04001D72 RID: 7538
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x04001D73 RID: 7539
		private const TargetIndex TargetCellIndex = TargetIndex.B;
	}
}
