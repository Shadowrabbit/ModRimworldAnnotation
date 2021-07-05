using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3B RID: 3387
	public static class StoreUtility
	{
		// Token: 0x06004D59 RID: 19801 RVA: 0x00036BFA File Offset: 0x00034DFA
		public static IHaulDestination CurrentHaulDestinationOf(Thing t)
		{
			if (t.Spawned)
			{
				return t.Map.haulDestinationManager.SlotGroupParentAt(t.Position);
			}
			return t.ParentHolder as IHaulDestination;
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x00036C26 File Offset: 0x00034E26
		public static StoragePriority CurrentStoragePriorityOf(Thing t)
		{
			return StoreUtility.StoragePriorityAtFor(StoreUtility.CurrentHaulDestinationOf(t), t);
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x00036C34 File Offset: 0x00034E34
		public static StoragePriority StoragePriorityAtFor(IntVec3 c, Thing t)
		{
			return StoreUtility.StoragePriorityAtFor(t.Map.haulDestinationManager.SlotGroupParentAt(c), t);
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x00036C4D File Offset: 0x00034E4D
		public static StoragePriority StoragePriorityAtFor(IHaulDestination at, Thing t)
		{
			if (at == null || !at.Accepts(t))
			{
				return StoragePriority.Unstored;
			}
			return at.GetStoreSettings().Priority;
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x00036C68 File Offset: 0x00034E68
		public static bool IsInAnyStorage(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) != null;
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x001AE6C0 File Offset: 0x001AC8C0
		public static bool IsInValidStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			return haulDestination != null && haulDestination.Accepts(t);
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x001AE6E0 File Offset: 0x001AC8E0
		public static bool IsInValidBestStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			IntVec3 intVec;
			IHaulDestination haulDestination2;
			return haulDestination != null && haulDestination.Accepts(t) && !StoreUtility.TryFindBestBetterStorageFor(t, null, t.Map, haulDestination.GetStoreSettings().Priority, Faction.OfPlayer, out intVec, out haulDestination2, false);
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x00036C73 File Offset: 0x00034E73
		public static Thing StoringThing(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) as Thing;
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x00036C80 File Offset: 0x00034E80
		public static SlotGroup GetSlotGroup(this Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return thing.Position.GetSlotGroup(thing.Map);
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x00036C9D File Offset: 0x00034E9D
		public static SlotGroup GetSlotGroup(this IntVec3 c, Map map)
		{
			if (map.haulDestinationManager == null)
			{
				return null;
			}
			return map.haulDestinationManager.SlotGroupAt(c);
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x001AE72C File Offset: 0x001AC92C
		public static bool IsValidStorageFor(this IntVec3 c, Map map, Thing storable)
		{
			if (!StoreUtility.NoStorageBlockersIn(c, map, storable))
			{
				return false;
			}
			SlotGroup slotGroup = c.GetSlotGroup(map);
			return slotGroup != null && slotGroup.parent.Accepts(storable);
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x001AE764 File Offset: 0x001AC964
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

		// Token: 0x06004D65 RID: 19813 RVA: 0x001AE804 File Offset: 0x001ACA04
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

		// Token: 0x06004D66 RID: 19814 RVA: 0x001AE8A4 File Offset: 0x001ACAA4
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

		// Token: 0x06004D67 RID: 19815 RVA: 0x001AE950 File Offset: 0x001ACB50
		public static bool TryFindBestBetterStoreCellForIn(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, SlotGroup slotGroup, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			foundCell = IntVec3.Invalid;
			float num = 2.1474836E+09f;
			StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref foundCell, ref num, ref currentPriority);
			return foundCell.IsValid;
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x001AE98C File Offset: 0x001ACB8C
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

		// Token: 0x06004D69 RID: 19817 RVA: 0x001AEA58 File Offset: 0x001ACC58
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
										goto IL_199;
									}
								}
								else if (faction != null && thing.IsForbidden(faction))
								{
									goto IL_199;
								}
							}
							if (thing != null)
							{
								if (carrier != null)
								{
									if (!carrier.CanReserveNew(thing))
									{
										goto IL_199;
									}
								}
								else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(thing, faction))
								{
									goto IL_199;
								}
							}
							if (carrier != null)
							{
								if (thing != null)
								{
									if (!carrier.Map.reachability.CanReach(intVec, thing, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false)))
									{
										goto IL_199;
									}
								}
								else if (!carrier.Map.reachability.CanReach(intVec, allHaulDestinationsListInPriorityOrder[i].Position, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false)))
								{
									goto IL_199;
								}
							}
							num = num2;
							storagePriority = priority;
							haulDestination = allHaulDestinationsListInPriorityOrder[i];
						}
					}
				}
				IL_199:;
			}
			return haulDestination != null;
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x001AEC18 File Offset: 0x001ACE18
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
			return carrier == null || carrier.Map.reachability.CanReach(t.SpawnedOrAnyParentSpawned ? t.PositionHeld : carrier.PositionHeld, c, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false));
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x001AECF0 File Offset: 0x001ACEF0
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
				if (intVec.InBounds(carrier.Map) && carrier.Map.areaManager.Home[intVec] && carrier.CanReach(intVec, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) && intVec.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(intVec, carrier.Map, item, carrier, carrier.Faction))
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
