using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073A RID: 1850
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		// Token: 0x06003354 RID: 13140 RVA: 0x00124E14 File Offset: 0x00123014
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x00124E2E File Offset: 0x0012302E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Wait(10, TargetIndex.None);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (!this.pawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					ThingCount firstUnloadableThing = this.pawn.inventory.FirstUnloadableThing;
					IntVec3 c;
					if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
					{
						Thing thing;
						this.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					this.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
					this.job.SetTarget(TargetIndex.B, c);
					this.countToDrop = firstUnloadableThing.Count;
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
					if (thing == null || !this.pawn.inventory.innerContainer.Contains(thing))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStorable(false))
					{
						this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, this.countToDrop, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, this.countToDrop, out thing, true);
						this.job.count = this.countToDrop;
						this.job.SetTarget(TargetIndex.A, thing);
					}
					thing.SetForbidden(false, false);
				}
			};
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04001E01 RID: 7681
		private int countToDrop = -1;

		// Token: 0x04001E02 RID: 7682
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		// Token: 0x04001E03 RID: 7683
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04001E04 RID: 7684
		private const int UnloadDuration = 10;
	}
}
