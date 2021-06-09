using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C0D RID: 3085
	public class JobDriver_UnloadInventory : JobDriver
	{
		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060048A6 RID: 18598 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x0003498D File Offset: 0x00032B8D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.OtherPawn, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x000349AF File Offset: 0x00032BAF
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
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x0400306A RID: 12394
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		// Token: 0x0400306B RID: 12395
		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		// Token: 0x0400306C RID: 12396
		private const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x0400306D RID: 12397
		private const int UnloadDuration = 10;
	}
}
