using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C06 RID: 3078
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004871 RID: 18545 RVA: 0x00195D88 File Offset: 0x00193F88
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004872 RID: 18546 RVA: 0x00190280 File Offset: 0x0018E480
		protected Thing Beer
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x00034781 File Offset: 0x00032981
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x000347A3 File Offset: 0x000329A3
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
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04003054 RID: 12372
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04003055 RID: 12373
		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		// Token: 0x04003056 RID: 12374
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x04003057 RID: 12375
		private const int Duration = 200;
	}
}
