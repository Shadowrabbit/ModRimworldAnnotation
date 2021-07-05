using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D2 RID: 1746
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x0011E25C File Offset: 0x0011C45C
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x0011E280 File Offset: 0x0011C480
		private Pawn Animal
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x0011E2A6 File Offset: 0x0011C4A6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x0011E2C8 File Offset: 0x0011C4C8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil findNearestCarrier = this.FindCarrierToil();
			yield return findNearestCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).JumpIf(() => !this.CanCarryAtLeastOne(this.Animal), findNearestCarrier);
			yield return this.GiveToCarrierAsMuchAsPossibleToil();
			yield return Toils_Jump.JumpIf(findNearestCarrier, () => this.pawn.carryTracker.CarriedThing != null);
			yield break;
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x0011E2D8 File Offset: 0x0011C4D8
		private Toil FindCarrierToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = this.FindCarrier();
					if (pawn == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					this.job.SetTarget(TargetIndex.B, pawn);
				}
			};
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x0011E2F4 File Offset: 0x0011C4F4
		private Pawn FindCarrier()
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(this.pawn);
			Pawn animal = this.Animal;
			if (animal != null && enumerable.Contains(animal) && animal.RaceProps.packAnimal && this.CanCarryAtLeastOne(animal))
			{
				return animal;
			}
			Pawn pawn = null;
			float num = -1f;
			foreach (Pawn pawn2 in enumerable)
			{
				if (pawn2.RaceProps.packAnimal && this.CanCarryAtLeastOne(pawn2))
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(this.pawn.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x0011E3BC File Offset: 0x0011C5BC
		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x0011E3CE File Offset: 0x0011C5CE
		private Toil GiveToCarrierAsMuchAsPossibleToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					if (this.Item == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(this.Animal, this.Item), this.Item.stackCount);
					this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.Item, this.Animal.inventory.innerContainer, count, true);
				}
			};
		}

		// Token: 0x04001D53 RID: 7507
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04001D54 RID: 7508
		private const TargetIndex AnimalInd = TargetIndex.B;
	}
}
