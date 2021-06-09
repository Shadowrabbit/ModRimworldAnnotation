﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000999 RID: 2457
	public class Toils_Haul
	{
		// Token: 0x06003C0A RID: 15370 RVA: 0x00170524 File Offset: 0x0016E724
		public static bool ErrorCheckForCarry(Pawn pawn, Thing haulThing)
		{
			if (!haulThing.Spawned)
			{
				Log.Message(string.Concat(new object[]
				{
					pawn,
					" tried to start carry ",
					haulThing,
					" which isn't spawned."
				}), false);
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return true;
			}
			if (haulThing.stackCount == 0)
			{
				Log.Message(string.Concat(new object[]
				{
					pawn,
					" tried to start carry ",
					haulThing,
					" which had stackcount 0."
				}), false);
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return true;
			}
			if (pawn.jobs.curJob.count <= 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Invalid count: ",
					pawn.jobs.curJob.count,
					", setting to 1. Job was ",
					pawn.jobs.curJob
				}), false);
				pawn.jobs.curJob.count = 1;
			}
			return false;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x00170620 File Offset: 0x0016E820
		public static Toil StartCarryThing(TargetIndex haulableInd, bool putRemainderInQueue = false, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(haulableInd).Thing;
				if (Toils_Haul.ErrorCheckForCarry(actor, thing))
				{
					return;
				}
				if (curJob.count == 0)
				{
					throw new Exception(string.Concat(new object[]
					{
						"StartCarryThing job had count = ",
						curJob.count,
						". Job: ",
						curJob
					}));
				}
				int num = actor.carryTracker.AvailableStackSpace(thing.def);
				if (num == 0)
				{
					throw new Exception(string.Concat(new object[]
					{
						"StartCarryThing got availableStackSpace ",
						num,
						" for haulTarg ",
						thing,
						". Job: ",
						curJob
					}));
				}
				if (failIfStackCountLessThanJobCount && thing.stackCount < curJob.count)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				int num2 = Mathf.Min(new int[]
				{
					curJob.count,
					num,
					thing.stackCount
				});
				if (num2 <= 0)
				{
					throw new Exception("StartCarryThing desiredNumToTake = " + num2);
				}
				int stackCount = thing.stackCount;
				int num3 = actor.carryTracker.TryStartCarry(thing, num2, true);
				if (num3 == 0)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				if (num3 < stackCount)
				{
					int num4 = curJob.count - num3;
					if (putRemainderInQueue && num4 > 0)
					{
						curJob.GetTargetQueue(haulableInd).Insert(0, thing);
						if (curJob.countQueue == null)
						{
							curJob.countQueue = new List<int>();
						}
						curJob.countQueue.Insert(0, num4);
					}
					else if (actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
					{
						actor.Map.reservationManager.Release(thing, actor, curJob);
					}
				}
				if (subtractNumTakenFromJobCount)
				{
					curJob.count -= num3;
				}
				curJob.SetTarget(haulableInd, actor.carryTracker.CarriedThing);
				actor.records.Increment(RecordDefOf.ThingsHauled);
			};
			return toil;
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x00170678 File Offset: 0x0016E878
		public static Toil JumpIfAlsoCollectingNextTargetInQueue(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				if (curJob.count <= 0)
				{
					return;
				}
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.", false);
					return;
				}
				if (actor.carryTracker.AvailableStackSpace(actor.carryTracker.CarriedThing.def) <= 0)
				{
					return;
				}
				for (int i = 0; i < targetQueue.Count; i++)
				{
					if (!GenAI.CanUseItemForWork(actor, targetQueue[i].Thing))
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					if (targetQueue[i].Thing.def == actor.carryTracker.CarriedThing.def)
					{
						curJob.SetTarget(ind, targetQueue[i].Thing);
						targetQueue.RemoveAt(i);
						actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
						return;
					}
				}
			};
			return toil;
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x001706C4 File Offset: 0x0016E8C4
		public static Toil CheckForGetOpportunityDuplicate(Toil getHaulTargetToil, TargetIndex haulableInd, TargetIndex storeCellInd, bool takeFromValidStorage = false, Predicate<Thing> extraValidator = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing.def.stackLimit == 1)
				{
					return;
				}
				if (actor.carryTracker.Full)
				{
					return;
				}
				if (curJob.count <= 0)
				{
					return;
				}
				Predicate<Thing> validator = (Thing t) => t.Spawned && t.def == actor.carryTracker.CarriedThing.def && t.CanStackWith(actor.carryTracker.CarriedThing) && !t.IsForbidden(actor) && (takeFromValidStorage || !t.IsInValidStorage()) && (storeCellInd == TargetIndex.None || curJob.GetTarget(storeCellInd).Cell.IsValidStorageFor(actor.Map, t)) && actor.CanReserve(t, 1, -1, null, false) && (extraValidator == null || extraValidator(t));
				Thing thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.ClosestTouch, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), 8f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (thing != null)
				{
					curJob.SetTarget(haulableInd, thing);
					actor.jobs.curDriver.JumpToToil(getHaulTargetToil);
				}
			};
			return toil;
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00170724 File Offset: 0x0016E924
		public static Toil CarryHauledThingToCell(TargetIndex squareIndex)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				IntVec3 cell = toil.actor.jobs.curJob.GetTarget(squareIndex).Cell;
				toil.actor.pather.StartPath(cell, PathEndMode.ClosestTouch);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.AddFailCondition(delegate
			{
				Pawn actor = toil.actor;
				IntVec3 cell = actor.jobs.curJob.GetTarget(squareIndex).Cell;
				return actor.jobs.curJob.haulMode == HaulMode.ToCellStorage && !cell.IsValidStorageFor(actor.Map, actor.carryTracker.CarriedThing);
			});
			return toil;
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x0017078C File Offset: 0x0016E98C
		public static Toil PlaceCarriedThingInCellFacing(TargetIndex facingTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in facing cell but is not hauling anything.", false);
					return;
				}
				LocalTargetInfo target = actor.CurJob.GetTarget(facingTargetInd);
				IntVec3 b;
				if (target.HasThing)
				{
					b = target.Thing.OccupiedRect().ClosestCellTo(actor.Position);
				}
				else
				{
					b = target.Cell;
				}
				IntVec3 dropLoc = actor.Position + Pawn_RotationTracker.RotFromAngleBiased((actor.Position - b).AngleFlat).FacingCell;
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(dropLoc, ThingPlaceMode.Direct, out thing, null))
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			return toil;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x001707D0 File Offset: 0x0016E9D0
		public static Toil PlaceHauledThingInCell(TargetIndex cellInd, Toil nextToilOnPlaceFailOrIncomplete, bool storageMode, bool tryStoreInSameStorageIfSpotCantHoldWholeStack = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				IntVec3 cell = curJob.GetTarget(cellInd).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in cell but is not hauling anything.", false);
					return;
				}
				SlotGroup slotGroup = actor.Map.haulDestinationManager.SlotGroupAt(cell);
				if (slotGroup != null && slotGroup.Settings.AllowedToAccept(actor.carryTracker.CarriedThing))
				{
					actor.Map.designationManager.TryRemoveDesignationOn(actor.carryTracker.CarriedThing, DesignationDefOf.Haul);
				}
				Action<Thing, int> placedAction = null;
				if (curJob.def == JobDefOf.DoBill || curJob.def == JobDefOf.RefuelAtomic || curJob.def == JobDefOf.RearmTurretAtomic)
				{
					placedAction = delegate(Thing th, int added)
					{
						if (curJob.placedThings == null)
						{
							curJob.placedThings = new List<ThingCountClass>();
						}
						ThingCountClass thingCountClass = curJob.placedThings.Find((ThingCountClass x) => x.thing == th);
						if (thingCountClass != null)
						{
							thingCountClass.Count += added;
							return;
						}
						curJob.placedThings.Add(new ThingCountClass(th, added));
					};
				}
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(cell, ThingPlaceMode.Direct, out thing, placedAction))
				{
					if (storageMode)
					{
						IntVec3 c;
						if (nextToilOnPlaceFailOrIncomplete != null && ((tryStoreInSameStorageIfSpotCantHoldWholeStack && StoreUtility.TryFindBestBetterStoreCellForIn(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, cell.GetSlotGroup(actor.Map), out c, true)) || StoreUtility.TryFindBestBetterStoreCellFor(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, out c, true)))
						{
							if (actor.CanReserve(c, 1, -1, null, false))
							{
								actor.Reserve(c, actor.CurJob, 1, -1, null, true);
							}
							actor.CurJob.SetTarget(cellInd, c);
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
						Job job = HaulAIUtility.HaulAsideJobFor(actor, actor.carryTracker.CarriedThing);
						if (job != null)
						{
							curJob.targetA = job.targetA;
							curJob.targetB = job.targetB;
							curJob.targetC = job.targetC;
							curJob.count = job.count;
							curJob.haulOpportunisticDuplicates = job.haulOpportunisticDuplicates;
							curJob.haulMode = job.haulMode;
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
						Log.Error(string.Concat(new object[]
						{
							"Incomplete haul for ",
							actor,
							": Could not find anywhere to put ",
							actor.carryTracker.CarriedThing,
							" near ",
							actor.Position,
							". Destroying. This should never happen!"
						}), false);
						actor.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
						return;
					}
					else if (nextToilOnPlaceFailOrIncomplete != null)
					{
						actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
						return;
					}
				}
			};
			return toil;
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x00170828 File Offset: 0x0016EA28
		public static Toil CarryHauledThingToContainer()
		{
			Toil gotoDest = new Toil();
			gotoDest.initAction = delegate()
			{
				gotoDest.actor.pather.StartPath(gotoDest.actor.jobs.curJob.targetB.Thing, PathEndMode.Touch);
			};
			gotoDest.AddFailCondition(delegate
			{
				Thing thing = gotoDest.actor.jobs.curJob.targetB.Thing;
				if (thing.Destroyed || (!gotoDest.actor.jobs.curJob.ignoreForbidden && thing.IsForbidden(gotoDest.actor)))
				{
					return true;
				}
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				return thingOwner != null && !thingOwner.CanAcceptAnyOf(gotoDest.actor.carryTracker.CarriedThing, true);
			});
			gotoDest.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return gotoDest;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x00170888 File Offset: 0x0016EA88
		public static Toil DepositHauledThingInContainer(TargetIndex containerInd, TargetIndex reserveForContainerInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in container but is not hauling anything.", false);
					return;
				}
				Thing thing = curJob.GetTarget(containerInd).Thing;
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null)
				{
					int num = actor.carryTracker.CarriedThing.stackCount;
					if (thing is IConstructible)
					{
						num = Mathf.Min(GenConstruct.AmountNeededByOf((IConstructible)thing, actor.carryTracker.CarriedThing.def), num);
						if (reserveForContainerInd != TargetIndex.None)
						{
							Thing thing2 = curJob.GetTarget(reserveForContainerInd).Thing;
							if (thing2 != null && thing2 != thing)
							{
								int num2 = GenConstruct.AmountNeededByOf((IConstructible)thing2, actor.carryTracker.CarriedThing.def);
								num = Mathf.Min(num, actor.carryTracker.CarriedThing.stackCount - num2);
							}
						}
					}
					if (actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, thingOwner, num, true) != 0)
					{
						Building_Grave building_Grave = thing as Building_Grave;
						if (building_Grave != null)
						{
							building_Grave.Notify_CorpseBuried(actor);
							return;
						}
					}
				}
				else
				{
					if (curJob.GetTarget(containerInd).Thing.def.Minifiable)
					{
						actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
						return;
					}
					Log.Error("Could not deposit hauled thing in container: " + curJob.GetTarget(containerInd).Thing, false);
				}
			};
			return toil;
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x001708D4 File Offset: 0x0016EAD4
		public static Toil JumpToCarryToNextContainerIfPossible(Toil carryToContainerToil, TargetIndex primaryTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					return;
				}
				if (curJob.targetQueueB != null && curJob.targetQueueB.Count > 0)
				{
					Thing primaryTarget = curJob.GetTarget(primaryTargetInd).Thing;
					bool hasSpareItems = actor.carryTracker.CarriedThing.stackCount > GenConstruct.AmountNeededByOf((IConstructible)primaryTarget, actor.carryTracker.CarriedThing.def);
					Predicate<ThingDefCountClass> <>9__4;
					Predicate<Thing> validator = delegate(Thing th)
					{
						if (!GenConstruct.CanConstruct(th, actor, false, false))
						{
							return false;
						}
						List<ThingDefCountClass> list = ((IConstructible)th).MaterialsNeeded();
						Predicate<ThingDefCountClass> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((ThingDefCountClass need) => need.thingDef == actor.carryTracker.CarriedThing.def));
						}
						return list.Any(predicate) && (th == primaryTarget || hasSpareItems);
					};
					Thing nextTarget = GenClosest.ClosestThing_Global_Reachable(actor.Position, actor.Map, from targ in curJob.targetQueueB
					select targ.Thing, PathEndMode.Touch, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), 99999f, validator, null);
					if (nextTarget != null)
					{
						curJob.targetQueueB.RemoveAll((LocalTargetInfo targ) => targ.Thing == nextTarget);
						curJob.targetB = nextTarget;
						actor.jobs.curDriver.JumpToToil(carryToContainerToil);
					}
				}
			};
			return toil;
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00170920 File Offset: 0x0016EB20
		public static Toil TakeToInventory(TargetIndex ind, int count)
		{
			return Toils_Haul.TakeToInventory(ind, () => count);
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x0017094C File Offset: 0x0016EB4C
		public static Toil TakeToInventory(TargetIndex ind, Func<int> countGetter)
		{
			Toil takeThing = new Toil();
			takeThing.initAction = delegate()
			{
				Pawn actor = takeThing.actor;
				Thing thing = actor.CurJob.GetTarget(ind).Thing;
				if (Toils_Haul.ErrorCheckForCarry(actor, thing))
				{
					return;
				}
				int num = Mathf.Min(countGetter(), thing.stackCount);
				if (actor.CurJob.checkEncumbrance)
				{
					num = Math.Min(num, MassUtility.CountToPickUpUntilOverEncumbered(actor, thing));
				}
				if (num <= 0)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.inventory.GetDirectlyHeldThings().TryAdd(thing.SplitOff(num), true);
				if (thing.def.ingestible != null && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
				{
					actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				}
				thing.def.soundPickup.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return takeThing;
		}
	}
}
