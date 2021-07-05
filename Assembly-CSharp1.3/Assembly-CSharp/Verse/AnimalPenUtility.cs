using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000175 RID: 373
	public static class AnimalPenUtility
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x00039098 File Offset: 0x00037298
		public static ThingFilter GetFixedAnimalFilter()
		{
			if (AnimalPenUtility.fixedFilter == null)
			{
				AnimalPenUtility.fixedFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.IsRopeManagedAnimalDef)))
				{
					AnimalPenUtility.fixedFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.fixedFilter;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00039110 File Offset: 0x00037310
		public static bool IsRopeManagedAnimalDef(ThingDef td)
		{
			return td.race != null && td.race.Roamer && td.IsWithinCategory(ThingCategoryDefOf.Animals);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00039134 File Offset: 0x00037334
		private static bool ShouldBePennedByDefault(ThingDef td)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(td) && td.race.FenceBlocked;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0003914B File Offset: 0x0003734B
		public static bool NeedsToBeManagedByRope(Pawn pawn)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(pawn.def) && pawn.Spawned && pawn.Map.IsPlayerHome;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00039170 File Offset: 0x00037370
		public static ThingFilter GetDefaultAnimalFilter()
		{
			if (AnimalPenUtility.defaultFilter == null)
			{
				AnimalPenUtility.defaultFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.ShouldBePennedByDefault)))
				{
					AnimalPenUtility.defaultFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.defaultFilter;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000391E8 File Offset: 0x000373E8
		public static void ResetStaticData()
		{
			AnimalPenUtility.defaultFilter = null;
			AnimalPenUtility.fixedFilter = null;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000391F8 File Offset: 0x000373F8
		public static CompAnimalPenMarker GetCurrentPenOf(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			if (!map.listerBuildings.allBuildingsAnimalPenMarkers.Any<Building>())
			{
				return null;
			}
			List<District> list = AnimalPenUtility.tmpConnectedDistrictsCalc.CalculateConnectedDistricts(animal.Position, map);
			if (!list.Any<District>())
			{
				return null;
			}
			CompAnimalPenMarker compAnimalPenMarker = null;
			foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				if (list.Contains(building.Position.GetDistrict(map, RegionType.Set_Passable)))
				{
					CompAnimalPenMarker compAnimalPenMarker2 = building.TryGetComp<CompAnimalPenMarker>();
					if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null) && (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || map.cellIndices.CellToIndex(compAnimalPenMarker2.parent.Position) < map.cellIndices.CellToIndex(compAnimalPenMarker.parent.Position)))
					{
						compAnimalPenMarker = compAnimalPenMarker2;
					}
				}
			}
			AnimalPenUtility.tmpConnectedDistrictsCalc.Reset();
			return compAnimalPenMarker;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00039310 File Offset: 0x00037510
		public static bool AnySuitablePens(Pawn animal, bool allowUnenclosedPens)
		{
			foreach (Building thing in animal.Map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker penMarker = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, penMarker, allowUnenclosedPens, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00039380 File Offset: 0x00037580
		public static CompAnimalPenMarker ClosestSuitablePen(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			CompAnimalPenMarker compAnimalPenMarker = null;
			float num = 0f;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null))
				{
					int num2 = animal.Position.DistanceToSquared(compAnimalPenMarker2.parent.Position);
					if (compAnimalPenMarker == null || (float)num2 < num)
					{
						compAnimalPenMarker = compAnimalPenMarker2;
						num = (float)num2;
					}
				}
			}
			return compAnimalPenMarker;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00039418 File Offset: 0x00037618
		private static bool CanUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper = null)
		{
			bool flag = false;
			return AnimalPenUtility.CheckUseAndReach(animal, penMarker, allowUnenclosedPens, roper, ref flag, ref flag, ref flag);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00039438 File Offset: 0x00037638
		private static bool CheckUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper, ref bool foundEnclosed, ref bool foundUsable, ref bool foundReachable)
		{
			if (!allowUnenclosedPens && penMarker.PenState.Unenclosed)
			{
				return false;
			}
			foundEnclosed = true;
			if (!penMarker.AcceptsToPen(animal))
			{
				return false;
			}
			if (roper == null && penMarker.parent.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (roper != null && penMarker.parent.IsForbidden(roper))
			{
				return false;
			}
			foundUsable = true;
			bool flag;
			if (roper == null)
			{
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams);
			}
			else
			{
				TraverseParms traverseParams2 = TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams2);
			}
			if (!flag)
			{
				return false;
			}
			foundReachable = true;
			return true;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00039514 File Offset: 0x00037714
		public static CompAnimalPenMarker GetPenAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false, bool canInteractWhileSleeping = true, bool allowUnenclosedPens = false, bool ignoreSkillRequirements = false, RopingPriority mode = RopingPriority.Closest)
		{
			jobFailReason = null;
			if (allowUnenclosedPens && mode == RopingPriority.Balanced)
			{
				Log.Warning("Cannot allow unenclosed pens in balanced mode");
				return null;
			}
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(animal, allowUnenclosedPens);
			if (mode == RopingPriority.Closest && currentPenOf != null && currentPenOf.PenState.Enclosed)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, canInteractWhileSleeping, ignoreSkillRequirements))
			{
				return null;
			}
			Map map = animal.Map;
			AnimalPenBalanceCalculator animalPenBalanceCalculator = new AnimalPenBalanceCalculator(map, true);
			CompAnimalPenMarker compAnimalPenMarker = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				flag2 = true;
				if (AnimalPenUtility.CheckUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, roper, ref flag3, ref flag4, ref flag5))
				{
					if (mode == RopingPriority.Closest)
					{
						if (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || AnimalPenUtility.PenIsCloser(compAnimalPenMarker2, compAnimalPenMarker, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
						}
					}
					else if (mode == RopingPriority.Balanced)
					{
						if (currentPenOf != null && !animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, currentPenOf, true, animal))
						{
							flag = true;
						}
						else if (compAnimalPenMarker == null || animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, compAnimalPenMarker, false, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
							flag = false;
						}
					}
				}
			}
			if (currentPenOf != null && compAnimalPenMarker == currentPenOf)
			{
				return null;
			}
			if (compAnimalPenMarker == null)
			{
				if (flag)
				{
					jobFailReason = "CantRopeAnimalAlreadyInBestPen".Translate();
				}
				else if (!flag2)
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				else if (!flag3)
				{
					jobFailReason = "CantRopeAnimalNoEnclosedPens".Translate();
				}
				else if (!flag4)
				{
					jobFailReason = "CantRopeAnimalNoAllowedPens".Translate();
				}
				else if (!flag5)
				{
					jobFailReason = "CantRopeAnimalNoReachablePens".Translate();
				}
				else
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				return null;
			}
			return compAnimalPenMarker;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00039764 File Offset: 0x00037964
		private static bool PenIsCloser(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, Pawn animal)
		{
			return animal.Position.DistanceToSquared(markerA.parent.Position) < animal.Position.DistanceToSquared(markerB.parent.Position);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00039794 File Offset: 0x00037994
		public static IntVec3 RopeAttachmentInteractionCell(Pawn roper, Pawn ropee)
		{
			if (!roper.Spawned || !ropee.Spawned)
			{
				return IntVec3.Invalid;
			}
			if (AnimalPenUtility.IsGoodRopeAttachmentInteractionCell(roper, ropee, roper.Position))
			{
				return roper.Position;
			}
			Map map = ropee.Map;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = ropee.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(map) && AnimalPenUtility.MutuallyWalkable(roper, ropee, intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00039814 File Offset: 0x00037A14
		public static bool IsGoodRopeAttachmentInteractionCell(Pawn roper, Pawn ropee, IntVec3 cell)
		{
			return ropee.Position.AdjacentToCardinal(cell) && AnimalPenUtility.MutuallyWalkable(roper, ropee, cell);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0003983C File Offset: 0x00037A3C
		private static bool MutuallyWalkable(Pawn roper, Pawn ropee, IntVec3 c)
		{
			Map map = ropee.Map;
			return c.WalkableBy(map, ropee) && c.WalkableBy(map, roper);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00039864 File Offset: 0x00037A64
		public static IntVec3 FindPlaceInPenToStand(CompAnimalPenMarker marker, Pawn pawn)
		{
			AnimalPenUtility.<>c__DisplayClass19_0 CS$<>8__locals1 = new AnimalPenUtility.<>c__DisplayClass19_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.marker = marker;
			CS$<>8__locals1.map = CS$<>8__locals1.pawn.Map;
			CS$<>8__locals1.result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(CS$<>8__locals1.marker.parent.Position, CS$<>8__locals1.map, (Region from, Region to) => CS$<>8__locals1.marker.PenState.ContainsConnectedRegion(to), new RegionProcessor(CS$<>8__locals1.<FindPlaceInPenToStand>g__RegionProcessor|1), 999999, RegionType.Set_Passable);
			return CS$<>8__locals1.result;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000398E4 File Offset: 0x00037AE4
		public static bool IsUnnecessarilyRoped(Pawn animal)
		{
			if (!AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return false;
			}
			if (animal.roping.IsRopedByPawn || !animal.roping.IsRopedToSpot)
			{
				return false;
			}
			Lord lord = animal.GetLord();
			int? num;
			if (lord == null)
			{
				num = null;
			}
			else
			{
				LordJob lordJob = lord.LordJob;
				num = ((lordJob != null) ? new bool?(lordJob.ManagesRopableAnimals) : null);
			}
			return (num ?? 0) == 0;
		}

		// Token: 0x040008E8 RID: 2280
		private static ThingFilter fixedFilter;

		// Token: 0x040008E9 RID: 2281
		private static ThingFilter defaultFilter;

		// Token: 0x040008EA RID: 2282
		private static readonly AnimalPenConnectedDistrictsCalculator tmpConnectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();
	}
}
