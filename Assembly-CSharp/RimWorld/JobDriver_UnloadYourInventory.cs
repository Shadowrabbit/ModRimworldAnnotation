using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C0F RID: 3087
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		// Token: 0x060048B3 RID: 18611 RVA: 0x000349E9 File Offset: 0x00032BE9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x00034A03 File Offset: 0x00032C03
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
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04003073 RID: 12403
		private int countToDrop = -1;

		// Token: 0x04003074 RID: 12404
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		// Token: 0x04003075 RID: 12405
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04003076 RID: 12406
		private const int UnloadDuration = 10;
	}
}
