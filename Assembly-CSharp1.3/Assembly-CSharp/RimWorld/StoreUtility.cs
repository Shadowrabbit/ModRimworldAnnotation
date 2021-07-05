﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000801 RID: 2049
	public static class StoreUtility
	{
		// Token: 0x060036A6 RID: 13990 RVA: 0x001362C0 File Offset: 0x001344C0
		public static IHaulDestination CurrentHaulDestinationOf(Thing t)
		{
			if (t.Spawned)
			{
				return t.Map.haulDestinationManager.SlotGroupParentAt(t.Position);
			}
			return t.ParentHolder as IHaulDestination;
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x001362EC File Offset: 0x001344EC
		public static StoragePriority CurrentStoragePriorityOf(Thing t)
		{
			return StoreUtility.StoragePriorityAtFor(StoreUtility.CurrentHaulDestinationOf(t), t);
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x001362FA File Offset: 0x001344FA
		public static StoragePriority StoragePriorityAtFor(IntVec3 c, Thing t)
		{
			return StoreUtility.StoragePriorityAtFor(t.Map.haulDestinationManager.SlotGroupParentAt(c), t);
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x00136313 File Offset: 0x00134513
		public static StoragePriority StoragePriorityAtFor(IHaulDestination at, Thing t)
		{
			if (at == null || !at.Accepts(t))
			{
				return StoragePriority.Unstored;
			}
			return at.GetStoreSettings().Priority;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x0013632E File Offset: 0x0013452E
		public static bool IsInAnyStorage(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) != null;
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x0013633C File Offset: 0x0013453C
		public static bool IsInValidStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			return haulDestination != null && haulDestination.Accepts(t);
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x0013635C File Offset: 0x0013455C
		public static bool IsInValidBestStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			IntVec3 intVec;
			IHaulDestination haulDestination2;
			return haulDestination != null && haulDestination.Accepts(t) && !StoreUtility.TryFindBestBetterStorageFor(t, null, t.Map, haulDestination.GetStoreSettings().Priority, Faction.OfPlayer, out intVec, out haulDestination2, false);
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x001363A5 File Offset: 0x001345A5
		public static Thing StoringThing(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) as Thing;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x001363B2 File Offset: 0x001345B2
		public static SlotGroup GetSlotGroup(this Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return thing.Position.GetSlotGroup(thing.Map);
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x001363CF File Offset: 0x001345CF
		public static SlotGroup GetSlotGroup(this IntVec3 c, Map map)
		{
			if (map.haulDestinationManager == null)
			{
				return null;
			}
			return map.haulDestinationManager.SlotGroupAt(c);
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x001363E8 File Offset: 0x001345E8
		public static bool IsValidStorageFor(this IntVec3 c, Map map, Thing storable)
		{
			if (!StoreUtility.NoStorageBlockersIn(c, map, storable))
			{
				return false;
			}
			SlotGroup slotGroup = c.GetSlotGroup(map);
			return slotGroup != null && slotGroup.parent.Accepts(storable);
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x00136420 File Offset: 0x00134620
		private static bool NoStorageBlockersIn(IntVec3 c, Map map, Thing thing)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.def.EverStorable(false))
				{
					if (!thing2.CanStackWith(thing))
					{
						return false;
					}
					if (thing2.stackCount >= thing.def.stackLimit)
					{
						return false;
					}
				}
				if (thing2.def.entityDefToBuild != null && thing2.def.entityDefToBuild.passability != Traversability.Standable)
				{
					return false;
				}
				if (thing2.def.surfaceType == SurfaceType.None && thing2.def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x001364C0 File Offset: 0x001346C0
		public static bool TryFindBestBetterStorageFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, out IHaulDestination haulDestination, bool needAccurateResult = true)
		{
			IntVec3 invalid = IntVec3.Invalid;
			StoragePriority storagePriority = StoragePriority.Unstored;
			if (StoreUtility.TryFindBestBetterStoreCellFor(t, carrier, map, currentPriority, faction, out invalid, needAccurateResult))
			{
				storagePriority = invalid.GetSlotGroup(map).Settings.Priority;
			}
			IHaulDestination haulDestination2;
			if (!StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(t, carrier, map, currentPriority, faction, out haulDestination2, false))
			{
				haulDestination2 = null;
			}
			if (storagePriority == StoragePriority.Unstored && haulDestination2 == null)
			{
				foundCell = IntVec3.Invalid;
				haulDestination = null;
				return false;
			}
			if (haulDestination2 != null && (storagePriority == StoragePriority.Unstored || haulDestination2.GetStoreSettings().Priority > storagePriority))
			{
				foundCell = IntVec3.Invalid;
				haulDestination = haulDestination2;
				return true;
			}
			foundCell = invalid;
			haulDestination = invalid.GetSlotGroup(map).parent;
			return true;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x00136560 File Offset: 0x00134760
		public static bool TryFindBestBetterStoreCellFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			List<SlotGroup> allGroupsListInPriorityOrder = map.haulDestinationManager.AllGroupsListInPriorityOrder;
			if (allGroupsListInPriorityOrder.Count == 0)
			{
				foundCell = IntVec3.Invalid;
				return false;
			}
			StoragePriority storagePriority = currentPriority;
			float num = 2.1474836E+09f;
			IntVec3 invalid = IntVec3.Invalid;
			int count = allGroupsListInPriorityOrder.Count;
			for (int i = 0; i < count; i++)
			{
				SlotGroup slotGroup = allGroupsListInPriorityOrder[i];
				StoragePriority priority = slotGroup.Settings.Priority;
				if (priority < storagePriority || priority <= currentPriority)
				{
					break;
				}
				StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref invalid, ref num, ref storagePriority);
			}
			if (!invalid.IsValid)
			{
				foundCell = IntVec3.Invalid;
				return false;
			}
			foundCell = invalid;
			return true;
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x0013660C File Offset: 0x0013480C
		public static bool TryFindBestBetterStoreCellForIn(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, SlotGroup slotGroup, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			foundCell = IntVec3.Invalid;
			float num = 2.1474836E+09f;
			StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref foundCell, ref num, ref currentPriority);
			return foundCell.IsValid;
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x00136648 File Offset: 0x00134848
		private static void TryFindBestBetterStoreCellForWorker(Thing t, Pawn carrier, Map map, Faction faction, SlotGroup slotGroup, bool needAccurateResult, ref IntVec3 closestSlot, ref float closestDistSquared, ref StoragePriority foundPriority)
		{
			if (slotGroup == null)
			{
				return;
			}
			if (!slotGroup.parent.Accepts(t))
			{
				return;
			}
			IntVec3 a = t.SpawnedOrAnyParentSpawned ? t.PositionHeld : carrier.PositionHeld;
			List<IntVec3> cellsList = slotGroup.CellsList;
			int count = cellsList.Count;
			int num;
			if (needAccurateResult)
			{
				num = Mathf.FloorToInt((float)count * Rand.Range(0.005f, 0.018f));
			}
			else
			{
				num = 0;
			}
			for (int i = 0; i < count; i++)
			{
				IntVec3 intVec = cellsList[i];
				float num2 = (float)(a - intVec).LengthHorizontalSquared;
				if (num2 <= closestDistSquared && StoreUtility.IsGoodStoreCell(intVec, map, t, carrier, faction))
				{
					closestSlot = intVec;
					closestDistSquared = num2;
					foundPriority = slotGroup.Settings.Priority;
					if (i >= num)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x00136714 File Offset: 0x00134914
		public static bool TryFindBestBetterNonSlotGroupStorageFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IHaulDestination haulDestination, bool acceptSamePriority = false)
		{
			List<IHaulDestination> allHaulDestinationsListInPriorityOrder = map.haulDestinationManager.AllHaulDestinationsListInPriorityOrder;
			IntVec3 intVec = t.SpawnedOrAnyParentSpawned ? t.PositionHeld : carrier.PositionHeld;
			float num = float.MaxValue;
			StoragePriority storagePriority = StoragePriority.Unstored;
			haulDestination = null;
			for (int i = 0; i < allHaulDestinationsListInPriorityOrder.Count; i++)
			{
				if (!(allHaulDestinationsListInPriorityOrder[i] is ISlotGroupParent))
				{
					StoragePriority priority = allHaulDestinationsListInPriorityOrder[i].GetStoreSettings().Priority;
					if (priority < storagePriority || (acceptSamePriority && priority < currentPriority) || (!acceptSamePriority && priority <= currentPriority))
					{
						break;
					}
					float num2 = (float)intVec.DistanceToSquared(allHaulDestinationsListInPriorityOrder[i].Position);
					if (num2 <= num && allHaulDestinationsListInPriorityOrder[i].Accepts(t))
					{
						Thing thing = allHaulDestinationsListInPriorityOrder[i] as Thing;
						if (thing == null || thing.Faction == faction)
						{
							if (thing != null)
							{
								if (carrier != null)
								{
									if (thing.IsForbidden(carrier))
									{
										goto IL_19D;
									}
								}
								else if (faction != null && thing.IsForbidden(faction))
								{
									goto IL_19D;
								}
							}
							if (thing != null)
							{
								if (carrier != null)
								{
									if (!carrier.CanReserveNew(thing))
									{
										goto IL_19D;
									}
								}
								else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(thing, faction))
								{
									goto IL_19D;
								}
							}
							if (carrier != null)
							{
								if (thing != null)
								{
									if (!carrier.Map.reachability.CanReach(intVec, thing, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false, false, false)))
									{
										goto IL_19D;
									}
								}
								else if (!carrier.Map.reachability.CanReach(intVec, allHaulDestinationsListInPriorityOrder[i].Position, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false, false, false)))
								{
									goto IL_19D;
								}
							}
							num = num2;
							storagePriority = priority;
							haulDestination = allHaulDestinationsListInPriorityOrder[i];
						}
					}
				}
				IL_19D:;
			}
			return haulDestination != null;
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x001368D8 File Offset: 0x00134AD8
		public static bool IsGoodStoreCell(IntVec3 c, Map map, Thing t, Pawn carrier, Faction faction)
		{
			if (carrier != null && c.IsForbidden(carrier))
			{
				return false;
			}
			if (!StoreUtility.NoStorageBlockersIn(c, map, t))
			{
				return false;
			}
			if (carrier != null)
			{
				if (!carrier.CanReserveNew(c))
				{
					return false;
				}
			}
			else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(c, faction))
			{
				return false;
			}
			if (c.ContainsStaticFire(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is IConstructible && GenConstruct.BlocksConstruction(thingList[i], t))
				{
					return false;
				}
			}
			return carrier == null || carrier.Map.reachability.CanReach(t.SpawnedOrAnyParentSpawned ? t.PositionHeld : carrier.PositionHeld, c, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false, false, false));
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x001369B0 File Offset: 0x00134BB0
		public static bool TryFindStoreCellNearColonyDesperate(Thing item, Pawn carrier, out IntVec3 storeCell)
		{
			if (StoreUtility.TryFindBestBetterStoreCellFor(item, carrier, carrier.Map, StoragePriority.Unstored, carrier.Faction, out storeCell, true))
			{
				return true;
			}
			for (int i = -4; i < 20; i++)
			{
				int num = (i < 0) ? Rand.RangeInclusive(0, 4) : i;
				IntVec3 intVec = carrier.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(carrier.Map) && carrier.Map.areaManager.Home[intVec] && carrier.CanReach(intVec, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) && intVec.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(intVec, carrier.Map, item, carrier, carrier.Faction))
				{
					storeCell = intVec;
					return true;
				}
			}
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(carrier.Position, carrier.Map, carrier, out storeCell, (IntVec3 x) => x.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(x, carrier.Map, item, carrier, carrier.Faction)))
			{
				return true;
			}
			storeCell = IntVec3.Invalid;
			return false;
		}
	}
}
