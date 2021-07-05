using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000735 RID: 1845
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600332F RID: 13103 RVA: 0x00124694 File Offset: 0x00122894
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x001246BC File Offset: 0x001228BC
		protected Thing Beer
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x001246DD File Offset: 0x001228DD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x001246FF File Offset: 0x001228FF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.Barrel.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.Barrel.TakeOutBeer();
					GenPlace.TryPlaceThing(thing, this.pawn.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
					IntVec3 c;
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
					{
						this.job.SetTarget(TargetIndex.C, c);
						this.job.SetTarget(TargetIndex.B, thing);
						this.job.count = thing.stackCount;
						return;
					}
					base.EndJobWith(JobCondition.Incompletable);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04001DF7 RID: 7671
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04001DF8 RID: 7672
		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		// Token: 0x04001DF9 RID: 7673
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x04001DFA RID: 7674
		private const int Duration = 200;
	}
}
