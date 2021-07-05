using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000587 RID: 1415
	public static class HaulAIUtility
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x000FA774 File Offset: 0x000F8974
		public static void Reset()
		{
			HaulAIUtility.ForbiddenLowerTrans = "ForbiddenLower".Translate();
			HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans = "ForbiddenOutsideAllowedAreaLower".Translate();
			HaulAIUtility.ReservedForPrisonersTrans = "ReservedForPrisoners".Translate();
			HaulAIUtility.BurningLowerTrans = "BurningLower".Translate();
			HaulAIUtility.NoEmptyPlaceLowerTrans = "NoEmptyPlaceLower".Translate();
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000FA7E8 File Offset: 0x000F89E8
		public static bool PawnCanAutomaticallyHaul(Pawn p, Thing t, bool forced)
		{
			if (!t.def.EverHaulable)
			{
				return false;
			}
			if (t.IsForbidden(p))
			{
				if (!t.Position.InAllowedArea(p))
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans, null);
				}
				else
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenLowerTrans, null);
				}
				return false;
			}
			return (t.def.alwaysHaulable || t.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul) != null || t.IsInValidStorage()) && HaulAIUtility.PawnCanAutomaticallyHaulFast(p, t, forced);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000FA874 File Offset: 0x000F8A74
		public static bool PawnCanAutomaticallyHaulFast(Pawn p, Thing t, bool forced)
		{
			UnfinishedThing unfinishedThing = t as UnfinishedThing;
			Building building;
			if (unfinishedThing != null && unfinishedThing.BoundBill != null && ((building = (unfinishedThing.BoundBill.billStack.billGiver as Building)) == null || (building.Spawned && building.OccupiedRect().ExpandedBy(1).Contains(unfinishedThing.Position))))
			{
				return false;
			}
			if (!p.CanReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			if (!p.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return false;
			}
			if (t.def.IsNutritionGivingIngestible && t.def.ingestible.HumanEdible && !t.IsSociallyProper(p, false, true))
			{
				JobFailReason.Is(HaulAIUtility.ReservedForPrisonersTrans, null);
				return false;
			}
			if (t.IsBurning())
			{
				JobFailReason.Is(HaulAIUtility.BurningLowerTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000FA96C File Offset: 0x000F8B6C
		public static Job HaulToStorageJob(Pawn p, Thing t)
		{
			StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(t);
			IntVec3 storeCell;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(t, p, p.Map, currentPriority, p.Faction, out storeCell, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			if (haulDestination is ISlotGroupParent)
			{
				return HaulAIUtility.HaulToCellStorageJob(p, t, storeCell, false);
			}
			Thing thing = haulDestination as Thing;
			if (thing != null && thing.TryGetInnerInteractableThingOwner() != null)
			{
				return HaulAIUtility.HaulToContainerJob(p, t, thing);
			}
			Log.Error("Don't know how to handle HaulToStorageJob for storage " + haulDestination.ToStringSafe<IHaulDestination>() + ". thing=" + t.ToStringSafe<Thing>());
			return null;
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000FA9F8 File Offset: 0x000F8BF8
		public static Job HaulToCellStorageJob(Pawn p, Thing t, IntVec3 storeCell, bool fitInStoreCell)
		{
			Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, storeCell);
			SlotGroup slotGroup = p.Map.haulDestinationManager.SlotGroupAt(storeCell);
			if (slotGroup != null)
			{
				Thing thing = p.Map.thingGrid.ThingAt(storeCell, t.def);
				if (thing != null)
				{
					job.count = t.def.stackLimit;
					if (fitInStoreCell)
					{
						job.count -= thing.stackCount;
					}
				}
				else
				{
					job.count = 99999;
				}
				int num = 0;
				float statValue = p.GetStatValue(StatDefOf.CarryingCapacity, true);
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					if (StoreUtility.IsGoodStoreCell(cellsList[i], p.Map, t, p, p.Faction))
					{
						Thing thing2 = p.Map.thingGrid.ThingAt(cellsList[i], t.def);
						if (thing2 != null && thing2 != t)
						{
							num += Mathf.Max(t.def.stackLimit - thing2.stackCount, 0);
						}
						else
						{
							num += t.def.stackLimit;
						}
						if (num >= job.count || (float)num >= statValue)
						{
							break;
						}
					}
				}
				job.count = Mathf.Min(job.count, num);
			}
			else
			{
				job.count = 99999;
			}
			job.haulOpportunisticDuplicates = true;
			job.haulMode = HaulMode.ToCellStorage;
			return job;
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000FAB68 File Offset: 0x000F8D68
		public static Job HaulToContainerJob(Pawn p, Thing t, Thing container)
		{
			ThingOwner thingOwner = container.TryGetInnerInteractableThingOwner();
			if (thingOwner == null)
			{
				Log.Error(container.ToStringSafe<Thing>() + " gave null ThingOwner.");
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer, t, container);
			job.count = Mathf.Min(t.stackCount, thingOwner.GetCountCanAccept(t, true));
			job.haulMode = HaulMode.ToContainer;
			return job;
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000FABCC File Offset: 0x000F8DCC
		public static bool CanHaulAside(Pawn p, Thing t, out IntVec3 storeCell)
		{
			storeCell = IntVec3.Invalid;
			return t.def.EverHaulable && !t.IsBurning() && p.CanReserveAndReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false) && HaulAIUtility.TryFindSpotToPlaceHaulableCloseTo(t, p, t.PositionHeld, out storeCell);
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000FAC2C File Offset: 0x000F8E2C
		public static Job HaulAsideJobFor(Pawn p, Thing t)
		{
			IntVec3 c;
			if (!HaulAIUtility.CanHaulAside(p, t, out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, c);
			job.count = 99999;
			job.haulOpportunisticDuplicates = false;
			job.haulMode = HaulMode.ToCellNonStorage;
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000FAC7C File Offset: 0x000F8E7C
		private static bool TryFindSpotToPlaceHaulableCloseTo(Thing haulable, Pawn worker, IntVec3 center, out IntVec3 spot)
		{
			Region region = center.GetRegion(worker.Map, RegionType.Set_Passable);
			if (region == null)
			{
				spot = center;
				return false;
			}
			TraverseParms traverseParms = TraverseParms.For(worker, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			IntVec3 foundCell = IntVec3.Invalid;
			Comparison<IntVec3> <>9__2;
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
			{
				HaulAIUtility.candidates.Clear();
				HaulAIUtility.candidates.AddRange(r.Cells);
				List<IntVec3> list = HaulAIUtility.candidates;
				Comparison<IntVec3> comparison;
				if ((comparison = <>9__2) == null)
				{
					comparison = (<>9__2 = ((IntVec3 a, IntVec3 b) => a.DistanceToSquared(center).CompareTo(b.DistanceToSquared(center))));
				}
				list.Sort(comparison);
				for (int i = 0; i < HaulAIUtility.candidates.Count; i++)
				{
					IntVec3 intVec = HaulAIUtility.candidates[i];
					if (HaulAIUtility.HaulablePlaceValidator(haulable, worker, intVec))
					{
						foundCell = intVec;
						return true;
					}
				}
				return false;
			}, 100, RegionType.Set_Passable);
			if (foundCell.IsValid)
			{
				spot = foundCell;
				return true;
			}
			spot = center;
			return false;
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000FAD3C File Offset: 0x000F8F3C
		private static bool HaulablePlaceValidator(Thing haulable, Pawn worker, IntVec3 c)
		{
			if (!worker.CanReserveAndReach(c, PathEndMode.OnCell, worker.NormalMaxDanger(), 1, -1, null, false))
			{
				return false;
			}
			if (GenPlace.HaulPlaceBlockerIn(haulable, c, worker.Map, true) != null)
			{
				return false;
			}
			if (!c.Standable(worker.Map))
			{
				return false;
			}
			if (c == haulable.Position && haulable.Spawned)
			{
				return false;
			}
			if (c.ContainsStaticFire(worker.Map))
			{
				return false;
			}
			if (haulable != null && haulable.def.BlocksPlanting(false) && worker.Map.zoneManager.ZoneAt(c) is Zone_Growing)
			{
				return false;
			}
			if (haulable.def.passability != Traversability.Standable)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[i];
					if (worker.Map.designationManager.DesignationAt(c2, DesignationDefOf.Mine) != null)
					{
						return false;
					}
				}
			}
			Building edifice = c.GetEdifice(worker.Map);
			return edifice == null || !(edifice is Building_Trap);
		}

		// Token: 0x040019AB RID: 6571
		private static string ForbiddenLowerTrans;

		// Token: 0x040019AC RID: 6572
		private static string ForbiddenOutsideAllowedAreaLowerTrans;

		// Token: 0x040019AD RID: 6573
		private static string ReservedForPrisonersTrans;

		// Token: 0x040019AE RID: 6574
		private static string BurningLowerTrans;

		// Token: 0x040019AF RID: 6575
		public static string NoEmptyPlaceLowerTrans;

		// Token: 0x040019B0 RID: 6576
		private static List<IntVec3> candidates = new List<IntVec3>();
	}
}
