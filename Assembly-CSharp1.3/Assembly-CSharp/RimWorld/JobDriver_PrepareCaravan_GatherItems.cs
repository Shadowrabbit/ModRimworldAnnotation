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
	// Token: 0x020006D4 RID: 1748
	public class JobDriver_PrepareCaravan_GatherItems : JobDriver
	{
		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x0011E5E8 File Offset: 0x0011C7E8
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x0011E60C File Offset: 0x0011C80C
		public Pawn Carrier
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x0011E632 File Offset: 0x0011C832
		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)this.job.lord.LordJob).transferables;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x0011E650 File Offset: 0x0011C850
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

		// Token: 0x060030B9 RID: 12473 RVA: 0x0011E67F File Offset: 0x0011C87F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ToHaul, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x0011E6A1 File Offset: 0x0011C8A1
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

		// Token: 0x060030BB RID: 12475 RVA: 0x0011E6B1 File Offset: 0x0011C8B1
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

		// Token: 0x060030BC RID: 12476 RVA: 0x0011E6D8 File Offset: 0x0011C8D8
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

		// Token: 0x060030BD RID: 12477 RVA: 0x0011E6FF File Offset: 0x0011C8FF
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

		// Token: 0x060030BE RID: 12478 RVA: 0x0011E718 File Offset: 0x0011C918
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

		// Token: 0x060030BF RID: 12479 RVA: 0x0011E734 File Offset: 0x0011C934
		public static bool IsUsableCarrier(Pawn p, Pawn forPawn, bool allowColonists)
		{
			return p.IsFormingCaravan() && (p == forPawn || (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach(p, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && ((allowColonists && p.IsColonist) || ((p.RaceProps.packAnimal || p.HostFaction == Faction.OfPlayer) && !p.IsBurning() && !p.Downed && !MassUtility.IsOverEncumbered(p)))));
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x0011E7C8 File Offset: 0x0011C9C8
		private float GetCarrierScore(Pawn p)
		{
			float lengthHorizontal = (p.Position - this.pawn.Position).LengthHorizontal;
			float num = MassUtility.EncumbrancePercent(p);
			return 1f - num - lengthHorizontal / 10f * 0.2f;
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x0011E810 File Offset: 0x0011CA10
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

		// Token: 0x04001D55 RID: 7509
		private const TargetIndex ToHaulInd = TargetIndex.A;

		// Token: 0x04001D56 RID: 7510
		private const TargetIndex CarrierInd = TargetIndex.B;

		// Token: 0x04001D57 RID: 7511
		private const int PlaceInInventoryDuration = 25;
	}
}
