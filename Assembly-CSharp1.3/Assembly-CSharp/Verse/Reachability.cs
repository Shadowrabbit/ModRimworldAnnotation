using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001F8 RID: 504
	public class Reachability
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x0004F97C File Offset: 0x0004DB7C
		public Reachability(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0004F9C9 File Offset: 0x0004DBC9
		public void ClearCache()
		{
			if (this.cache.Count > 0)
			{
				this.cache.Clear();
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0004F9E4 File Offset: 0x0004DBE4
		public void ClearCacheFor(Pawn pawn)
		{
			this.cache.ClearFor(pawn);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0004F9F2 File Offset: 0x0004DBF2
		public void ClearCacheForHostile(Thing hostileTo)
		{
			this.cache.ClearForHostile(hostileTo);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0004FA00 File Offset: 0x0004DC00
		private void QueueNewOpenRegion(Region region)
		{
			if (region == null)
			{
				Log.ErrorOnce("Tried to queue null region.", 881121);
				return;
			}
			if (region.reachedIndex == this.reachedIndex)
			{
				Log.ErrorOnce("Region is already reached; you can't open it. Region: " + region.ToString(), 719991);
				return;
			}
			this.openQueue.Enqueue(region);
			region.reachedIndex = this.reachedIndex;
			this.numRegionsOpened++;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0004FA70 File Offset: 0x0004DC70
		private uint NewReachedIndex()
		{
			uint num = this.reachedIndex;
			this.reachedIndex = num + 1U;
			return num;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0004FA8E File Offset: 0x0004DC8E
		private void FinalizeCheck()
		{
			this.working = false;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0004FA97 File Offset: 0x0004DC97
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseMode, maxDanger);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0004FAC5 File Offset: 0x0004DCC5
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseParams);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0004FAF4 File Offset: 0x0004DCF4
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return this.CanReach(start, dest, peMode, TraverseParms.For(traverseMode, maxDanger, false, false, false));
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0004FB18 File Offset: 0x0004DD18
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			if (this.working)
			{
				Log.ErrorOnce("Called CanReach() while working. This should never happen. Suppressing further errors.", 7312233);
				return false;
			}
			if (traverseParams.pawn != null)
			{
				if (!traverseParams.pawn.Spawned)
				{
					return false;
				}
				if (traverseParams.pawn.Map != this.map)
				{
					Log.Error(string.Concat(new object[]
					{
						"Called CanReach() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=",
						traverseParams.pawn,
						" pawn.Map=",
						traverseParams.pawn.Map,
						" map=",
						this.map
					}));
					return false;
				}
			}
			if (ReachabilityImmediate.CanReachImmediate(start, dest, this.map, peMode, traverseParams.pawn))
			{
				return true;
			}
			if (!dest.IsValid)
			{
				return false;
			}
			if (dest.HasThing && dest.Thing.Map != this.map)
			{
				return false;
			}
			if (!start.InBounds(this.map) || !dest.Cell.InBounds(this.map))
			{
				return false;
			}
			if ((peMode == PathEndMode.OnCell || peMode == PathEndMode.Touch || peMode == PathEndMode.ClosestTouch) && traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
			{
				District district = RegionAndRoomQuery.DistirctAtFast(start, this.map, RegionType.Set_Passable);
				if (district != null && district == RegionAndRoomQuery.DistirctAtFast(dest.Cell, this.map, RegionType.Set_Passable))
				{
					return true;
				}
			}
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				TraverseParms traverseParams2 = traverseParams;
				traverseParams2.mode = TraverseMode.PassDoors;
				if (this.CanReach(start, dest, peMode, traverseParams2))
				{
					return true;
				}
			}
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(traverseParams.pawn, dest.ToTargetInfo(this.map), ref peMode);
			this.working = true;
			bool result;
			try
			{
				this.pathGrid = this.map.pathing.For(traverseParams).pathGrid;
				this.regionGrid = this.map.regionGrid;
				this.reachedIndex += 1U;
				this.destRegions.Clear();
				if (peMode == PathEndMode.OnCell)
				{
					Region region = dest.Cell.GetRegion(this.map, RegionType.Set_Passable);
					if (region != null && region.Allows(traverseParams, true))
					{
						this.destRegions.Add(region);
					}
				}
				else if (peMode == PathEndMode.Touch)
				{
					TouchPathEndModeUtility.AddAllowedAdjacentRegions(dest, traverseParams, this.map, this.destRegions);
				}
				if (this.destRegions.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
				{
					this.FinalizeCheck();
					result = false;
				}
				else
				{
					this.destRegions.RemoveDuplicates(null);
					this.openQueue.Clear();
					this.numRegionsOpened = 0;
					this.DetermineStartRegions(start);
					if (this.openQueue.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
					{
						this.FinalizeCheck();
						result = false;
					}
					else
					{
						if (this.startingRegions.Any<Region>() && this.destRegions.Any<Region>() && this.CanUseCache(traverseParams.mode))
						{
							switch (this.GetCachedResult(traverseParams))
							{
							case BoolUnknown.True:
								this.FinalizeCheck();
								return true;
							case BoolUnknown.False:
								this.FinalizeCheck();
								return false;
							}
						}
						if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater)
						{
							bool flag = this.CheckCellBasedReachability(start, dest, peMode, traverseParams);
							this.FinalizeCheck();
							result = flag;
						}
						else
						{
							bool flag2 = this.CheckRegionBasedReachability(traverseParams);
							this.FinalizeCheck();
							result = flag2;
						}
					}
				}
			}
			finally
			{
				this.working = false;
			}
			return result;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0004FE90 File Offset: 0x0004E090
		private void DetermineStartRegions(IntVec3 start)
		{
			this.startingRegions.Clear();
			if (this.pathGrid.WalkableFast(start))
			{
				Region validRegionAt = this.regionGrid.GetValidRegionAt(start);
				this.QueueNewOpenRegion(validRegionAt);
				this.startingRegions.Add(validRegionAt);
				return;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = start + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(this.map) && this.pathGrid.WalkableFast(intVec))
				{
					Region validRegionAt2 = this.regionGrid.GetValidRegionAt(intVec);
					if (validRegionAt2 != null && validRegionAt2.reachedIndex != this.reachedIndex)
					{
						this.QueueNewOpenRegion(validRegionAt2);
						this.startingRegions.Add(validRegionAt2);
					}
				}
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0004FF44 File Offset: 0x0004E144
		private BoolUnknown GetCachedResult(TraverseParms traverseParams)
		{
			bool flag = false;
			for (int i = 0; i < this.startingRegions.Count; i++)
			{
				for (int j = 0; j < this.destRegions.Count; j++)
				{
					if (this.destRegions[j] == this.startingRegions[i])
					{
						return BoolUnknown.True;
					}
					BoolUnknown boolUnknown = this.cache.CachedResultFor(this.startingRegions[i].District, this.destRegions[j].District, traverseParams);
					if (boolUnknown == BoolUnknown.True)
					{
						return BoolUnknown.True;
					}
					if (boolUnknown == BoolUnknown.Unknown)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.Unknown;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0004FFDC File Offset: 0x0004E1DC
		private bool CheckRegionBasedReachability(TraverseParms traverseParams)
		{
			while (this.openQueue.Count > 0)
			{
				Region region = this.openQueue.Dequeue();
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					for (int j = 0; j < 2; j++)
					{
						Region region2 = regionLink.regions[j];
						if (region2 != null && region2.reachedIndex != this.reachedIndex && region2.type.Passable() && region2.Allows(traverseParams, false))
						{
							if (this.destRegions.Contains(region2))
							{
								for (int k = 0; k < this.startingRegions.Count; k++)
								{
									this.cache.AddCachedResult(this.startingRegions[k].District, region2.District, traverseParams, true);
								}
								return true;
							}
							this.QueueNewOpenRegion(region2);
						}
					}
				}
			}
			for (int l = 0; l < this.startingRegions.Count; l++)
			{
				for (int m = 0; m < this.destRegions.Count; m++)
				{
					this.cache.AddCachedResult(this.startingRegions[l].District, this.destRegions[m].District, traverseParams, false);
				}
			}
			return false;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00050140 File Offset: 0x0004E340
		private bool CheckCellBasedReachability(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			IntVec3 foundCell = IntVec3.Invalid;
			Region[] directRegionGrid = this.regionGrid.DirectGrid;
			PathGrid pathGrid = this.map.pathing.For(traverseParams).pathGrid;
			CellIndices cellIndices = this.map.cellIndices;
			this.map.floodFiller.FloodFill(start, delegate(IntVec3 c)
			{
				int num = cellIndices.CellToIndex(c);
				if ((traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater) && c.GetTerrain(this.map).IsWater)
				{
					return false;
				}
				if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
				{
					if (!pathGrid.WalkableFast(num))
					{
						Building edifice = c.GetEdifice(this.map);
						if (edifice == null || !PathFinder.IsDestroyable(edifice))
						{
							return false;
						}
					}
				}
				else if (traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater)
				{
					Log.ErrorOnce("Do not use this method for non-cell based modes!", 938476762);
					if (!pathGrid.WalkableFast(num))
					{
						return false;
					}
				}
				Region region = directRegionGrid[num];
				return region == null || region.Allows(traverseParams, false);
			}, delegate(IntVec3 c)
			{
				if (ReachabilityImmediate.CanReachImmediate(c, dest, this.map, peMode, traverseParams.pawn))
				{
					foundCell = c;
					return true;
				}
				return false;
			}, int.MaxValue, false, null);
			if (foundCell.IsValid)
			{
				if (this.CanUseCache(traverseParams.mode))
				{
					Region validRegionAt = this.regionGrid.GetValidRegionAt(foundCell);
					if (validRegionAt != null)
					{
						for (int i = 0; i < this.startingRegions.Count; i++)
						{
							this.cache.AddCachedResult(this.startingRegions[i].District, validRegionAt.District, traverseParams, true);
						}
					}
				}
				return true;
			}
			if (this.CanUseCache(traverseParams.mode))
			{
				for (int j = 0; j < this.startingRegions.Count; j++)
				{
					for (int k = 0; k < this.destRegions.Count; k++)
					{
						this.cache.AddCachedResult(this.startingRegions[j].District, this.destRegions[k].District, traverseParams, false);
					}
				}
			}
			return false;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000502DD File Offset: 0x0004E4DD
		public bool CanReachColony(IntVec3 c)
		{
			return this.CanReachFactionBase(c, Faction.OfPlayer);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x000502EC File Offset: 0x0004E4EC
		public bool CanReachFactionBase(IntVec3 c, Faction factionBaseFaction)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return this.CanReach(c, MapGenerator.PlayerStartSpot, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false));
			}
			if (!c.Walkable(this.map))
			{
				return false;
			}
			Faction faction = this.map.ParentFaction ?? Faction.OfPlayer;
			List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
			if (faction == Faction.OfPlayer)
			{
				List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
				for (int j = 0; j < allBuildingsColonist.Count; j++)
				{
					if (this.CanReach(c, allBuildingsColonist[j], PathEndMode.Touch, traverseParams))
					{
						return true;
					}
				}
			}
			else
			{
				List<Thing> list2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int k = 0; k < list2.Count; k++)
				{
					if (list2[k].Faction == faction && this.CanReach(c, list2[k], PathEndMode.Touch, traverseParams))
					{
						return true;
					}
				}
			}
			return this.CanReachBiggestMapEdgeDistrict(c);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00050438 File Offset: 0x0004E638
		public bool CanReachBiggestMapEdgeDistrict(IntVec3 c)
		{
			District district = null;
			for (int i = 0; i < this.map.regionGrid.allDistricts.Count; i++)
			{
				District district2 = this.map.regionGrid.allDistricts[i];
				if (district2.TouchesMapEdge && (district == null || district2.RegionCount > district.RegionCount))
				{
					district = district2;
				}
			}
			return district != null && this.CanReach(c, district.Regions[0].AnyCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false));
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000504C8 File Offset: 0x0004E6C8
		public bool CanReachMapEdge(IntVec3 c, TraverseParms traverseParms)
		{
			if (traverseParms.pawn != null)
			{
				if (!traverseParms.pawn.Spawned)
				{
					return false;
				}
				if (traverseParms.pawn.Map != this.map)
				{
					Log.Error(string.Concat(new object[]
					{
						"Called CanReachMapEdge() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=",
						traverseParms.pawn,
						" pawn.Map=",
						traverseParms.pawn.Map,
						" map=",
						this.map
					}));
					return false;
				}
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			if (region.District.TouchesMapEdge)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			bool foundReg = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.District.TouchesMapEdge)
				{
					foundReg = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			return foundReg;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000505CC File Offset: 0x0004E7CC
		public bool CanReachUnfogged(IntVec3 c, TraverseParms traverseParms)
		{
			if (traverseParms.pawn != null)
			{
				if (!traverseParms.pawn.Spawned)
				{
					return false;
				}
				if (traverseParms.pawn.Map != this.map)
				{
					Log.Error(string.Concat(new object[]
					{
						"Called CanReachUnfogged() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=",
						traverseParms.pawn,
						" pawn.Map=",
						traverseParms.pawn.Map,
						" map=",
						this.map
					}));
					return false;
				}
			}
			if (!c.InBounds(this.map))
			{
				return false;
			}
			if (!c.Fogged(this.map))
			{
				return true;
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			bool foundReg = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (!r.AnyCell.Fogged(this.map))
				{
					foundReg = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			return foundReg;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000506E7 File Offset: 0x0004E8E7
		private bool CanUseCache(TraverseMode mode)
		{
			return mode != TraverseMode.PassAllDestroyableThingsNotWater && mode != TraverseMode.NoPassClosedDoorsOrWater;
		}

		// Token: 0x04000B85 RID: 2949
		private Map map;

		// Token: 0x04000B86 RID: 2950
		private Queue<Region> openQueue = new Queue<Region>();

		// Token: 0x04000B87 RID: 2951
		private List<Region> startingRegions = new List<Region>();

		// Token: 0x04000B88 RID: 2952
		private List<Region> destRegions = new List<Region>();

		// Token: 0x04000B89 RID: 2953
		private uint reachedIndex = 1U;

		// Token: 0x04000B8A RID: 2954
		private int numRegionsOpened;

		// Token: 0x04000B8B RID: 2955
		private bool working;

		// Token: 0x04000B8C RID: 2956
		private ReachabilityCache cache = new ReachabilityCache();

		// Token: 0x04000B8D RID: 2957
		private PathGrid pathGrid;

		// Token: 0x04000B8E RID: 2958
		private RegionGrid regionGrid;
	}
}
