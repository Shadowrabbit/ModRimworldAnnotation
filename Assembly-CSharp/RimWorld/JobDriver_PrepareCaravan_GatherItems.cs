using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B46 RID: 2886
	public class JobDriver_PrepareCaravan_GatherItems : JobDriver
	{
		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x060043CC RID: 17356 RVA: 0x0018EA98 File Offset: 0x0018CC98
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x060043CD RID: 17357 RVA: 0x0018EABC File Offset: 0x0018CCBC
		public Pawn Carrier
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x060043CE RID: 17358 RVA: 0x00032335 File Offset: 0x00030535
		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)this.job.lord.LordJob).transferables;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x060043CF RID: 17359 RVA: 0x0018EEF8 File Offset: 0x0018D0F8
		private TransferableOneWay Transferable
		{
			get
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(this.ToHaul, this.Transferables, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null)
				{
					return transferableOneWay;
				}
				throw new InvalidOperationException("Could not find any matching transferable.");
			}
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x00032351 File Offset: 0x00030551
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ToHaul, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x00032373 File Offset: 0x00030573
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null).FailOnDespawnedOrNull(TargetIndex.A);
			yield return reserve;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return this.DetermineNumToHaul();
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			yield return this.AddCarriedThingToTransferables();
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserve, TargetIndex.A, TargetIndex.None, true, (Thing x) => this.Transferable.things.Contains(x));
			Toil findCarrier = this.FindCarrier();
			yield return findCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier);
			yield return Toils_General.Wait(25, TargetIndex.None).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return this.PlaceTargetInCarrierInventory();
			yield break;
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x00032383 File Offset: 0x00030583
		private Toil DetermineNumToHaul()
		{
			return new Toil
			{
				initAction = delegate()
				{
					int num = GatherItemsForCaravanUtility.CountLeftToTransfer(this.pawn, this.Transferable, this.job.lord);
					if (this.pawn.carryTracker.CarriedThing != null)
					{
						num -= this.pawn.carryTracker.CarriedThing.stackCount;
					}
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

		// Token: 0x060043D3 RID: 17363 RVA: 0x000323AA File Offset: 0x000305AA
		private Toil AddCarriedThingToTransferables()
		{
			return new Toil
			{
				initAction = delegate()
				{
					TransferableOneWay transferable = this.Transferable;
					if (!transferable.things.Contains(this.pawn.carryTracker.CarriedThing))
					{
						transferable.things.Add(this.pawn.carryTracker.CarriedThing);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x000323D1 File Offset: 0x000305D1
		private Toil FindCarrier()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = this.FindBestCarrier(true);
					if (pawn == null)
					{
						bool flag = this.pawn.GetLord() == this.job.lord;
						if (flag && !MassUtility.IsOverEncumbered(this.pawn))
						{
							pawn = this.pawn;
						}
						else
						{
							pawn = this.FindBestCarrier(false);
							if (pawn == null)
							{
								if (flag)
								{
									pawn = this.pawn;
								}
								else
								{
									IEnumerable<Pawn> source = from x in this.job.lord.ownedPawns
									where JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(x, this.pawn, true)
									select x;
									if (!source.Any<Pawn>())
									{
										base.EndJobWith(JobCondition.Incompletable);
										return;
									}
									pawn = source.RandomElement<Pawn>();
								}
							}
						}
					}
					this.job.SetTarget(TargetIndex.B, pawn);
				}
			};
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x000323EA File Offset: 0x000305EA
		private Toil PlaceTargetInCarrierInventory()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
					Thing carriedThing = carryTracker.CarriedThing;
					this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - carriedThing.stackCount, 0));
					carryTracker.innerContainer.TryTransferToContainer(carriedThing, this.Carrier.inventory.innerContainer, carriedThing.stackCount, true);
				}
			};
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0018EF28 File Offset: 0x0018D128
		public static bool IsUsableCarrier(Pawn p, Pawn forPawn, bool allowColonists)
		{
			return p.IsFormingCaravan() && (p == forPawn || (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach(p, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && ((allowColonists && p.IsColonist) || ((p.RaceProps.packAnimal || p.HostFaction == Faction.OfPlayer) && !p.IsBurning() && !p.Downed && !MassUtility.IsOverEncumbered(p)))));
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0018EFB8 File Offset: 0x0018D1B8
		private float GetCarrierScore(Pawn p)
		{
			float lengthHorizontal = (p.Position - this.pawn.Position).LengthHorizontal;
			float num = MassUtility.EncumbrancePercent(p);
			return 1f - num - lengthHorizontal / 10f * 0.2f;
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0018F000 File Offset: 0x0018D200
		private Pawn FindBestCarrier(bool onlyAnimals)
		{
			Lord lord = this.job.lord;
			Pawn pawn = null;
			float num = 0f;
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					if (pawn2 != this.pawn && (!onlyAnimals || pawn2.RaceProps.Animal) && JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(pawn2, this.pawn, false))
					{
						float carrierScore = this.GetCarrierScore(pawn2);
						if (pawn == null || carrierScore > num)
						{
							pawn = pawn2;
							num = carrierScore;
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x04002E36 RID: 11830
		private const TargetIndex ToHaulInd = TargetIndex.A;

		// Token: 0x04002E37 RID: 11831
		private const TargetIndex CarrierInd = TargetIndex.B;

		// Token: 0x04002E38 RID: 11832
		private const int PlaceInInventoryDuration = 25;
	}
}
