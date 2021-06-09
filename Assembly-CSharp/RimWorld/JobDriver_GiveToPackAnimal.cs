using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B42 RID: 2882
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x0018EA98 File Offset: 0x0018CC98
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x0018EABC File Offset: 0x0018CCBC
		private Pawn Animal
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0003223C File Offset: 0x0003043C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0003225E File Offset: 0x0003045E
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

		// Token: 0x060043B5 RID: 17333 RVA: 0x0003226E File Offset: 0x0003046E
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

		// Token: 0x060043B6 RID: 17334 RVA: 0x0018EAE4 File Offset: 0x0018CCE4
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

		// Token: 0x060043B7 RID: 17335 RVA: 0x00032287 File Offset: 0x00030487
		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x00032299 File Offset: 0x00030499
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

		// Token: 0x04002E2C RID: 11820
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04002E2D RID: 11821
		private const TargetIndex AnimalInd = TargetIndex.B;
	}
}
