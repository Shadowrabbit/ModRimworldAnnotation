using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000739 RID: 1849
	public class JobDriver_UnloadInventory : JobDriver
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x00124C90 File Offset: 0x00122E90
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x00124CB6 File Offset: 0x00122EB6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.OtherPawn, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00124CD8 File Offset: 0x00122ED8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(10, TargetIndex.None);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn otherPawn = this.OtherPawn;
					if (!otherPawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
					IntVec3 c;
					if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
					{
						Thing thing;
						otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
						if (thing != null)
						{
							thing.SetForbidden(false, false);
							return;
						}
					}
					else
					{
						Thing thing2;
						otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
						this.job.count = thing2.stackCount;
						this.job.SetTarget(TargetIndex.B, thing2);
						this.job.SetTarget(TargetIndex.C, c);
						firstUnloadableThing.Thing.SetForbidden(false, false);
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04001DFD RID: 7677
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		// Token: 0x04001DFE RID: 7678
		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		// Token: 0x04001DFF RID: 7679
		private const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x04001E00 RID: 7680
		private const int UnloadDuration = 10;
	}
}
