using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class JobDriver_GiveToPawn : JobDriver
	{
		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x001226AC File Offset: 0x001208AC
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x001226D0 File Offset: 0x001208D0
		private Pawn Pawn
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Pawn;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x001226F1 File Offset: 0x001208F1
		public int CountBeingHauled
		{
			get
			{
				if (this.pawn.carryTracker.CarriedThing == null)
				{
					return 0;
				}
				return this.pawn.carryTracker.CarriedThing.stackCount;
			}
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x0012271C File Offset: 0x0012091C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0012273E File Offset: 0x0012093E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !GiveItemsToPawnUtility.IsWaitingForItems(this.Pawn));
			Toil setItemTarget = this.SetItemTarget();
			yield return setItemTarget;
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null).FailOnDespawnedOrNull(TargetIndex.A);
			yield return reserve;
			yield return this.DetermineNumToHaul();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil checkForDuplicates = Toils_Haul.CheckForGetOpportunityDuplicate(reserve, TargetIndex.A, TargetIndex.None, true, (Thing x) => x.def == this.Item.def);
			Toil haulToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return Toils_Jump.JumpIf(haulToContainer, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Pawn) <= 0);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return Toils_Jump.JumpIf(haulToContainer, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Pawn) <= 0);
			yield return checkForDuplicates;
			yield return haulToContainer;
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.B);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A, null);
			yield return Toils_Jump.JumpIf(setItemTarget, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Pawn) > 0);
			yield break;
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x0012274E File Offset: 0x0012094E
		private Toil DetermineNumToHaul()
		{
			return new Toil
			{
				initAction = delegate()
				{
					int num = GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Pawn);
					if (num <= 0)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
						return;
					}
					this.job.count = num;
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x00122775 File Offset: 0x00120975
		private Toil SetItemTarget()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Thing thing = GiveItemsToPawnUtility.FindItemToGive(this.pawn, this.Item.def);
					if (thing == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					this.job.SetTarget(TargetIndex.A, thing);
				}
			};
		}

		// Token: 0x04001DC1 RID: 7617
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04001DC2 RID: 7618
		private const TargetIndex PawnInd = TargetIndex.B;
	}
}
