using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002C6 RID: 710
	public class Reachability
	{
		// Token: 0x060011F4 RID: 4596 RVA: 0x000C3ECC File Offset: 0x000C20CC
		public Reachability(Map map)
		{
			this.map = map;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00012FAB File Offset: 0x000111AB
		public void ClearCache()
		{
			if (this.cache.Count > 0)
			{
				this.cache.Clear();
			}
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00012FC6 File Offset: 0x000111C6
		public void ClearCacheFor(Pawn pawn)
		{
			this.cache.ClearFor(pawn);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00012FD4 File Offset: 0x000111D4
		public void ClearCacheForHostile(Thing hostileTo)
		{
			this.cache.ClearForHostile(hostileTo);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000C3F1C File Offset: 0x000C211C
		private void QueueNewOpenRegion(Region region)
		{
			if (region == null)
			{
				Log.ErrorOnce("Tried to queue null region.", 881121, false);
				return;
			}
			if (region.reachedIndex == this.reachedIndex)
			{
				Log.ErrorOnce("Region is already reached; you can't open it. Region: " + region.ToString(), 719991, false);
				return;
			}
			this.openQueue.Enqueue(region);
			region.reachedIndex = this.reachedIndex;
			this.numRegionsOpened++;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000C3F90 File Offset: 0x000C2190
		private uint NewReachedIndex()
		{
			uint num = this.reachedIndex;
			this.reachedIndex = num + 1U;
			return num;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00012FE2 File Offset: 0x000111E2
		private void FinalizeCheck()
		{
			this.working = false;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00012FEB File Offset: 0x000111EB
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseMode, maxDanger);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00013019 File Offset: 0x00011219
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseParams);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00013045 File Offset: 0x00011245
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return this.CanReach(start, dest, peMode, TraverseParms.For(traverseMode, maxDanger, false));
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000C3FB0 File Offset: 0x000C21B0
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			if (this.working)
			{
				Log.ErrorOnce("Called CanReach() while working. This should never happen. Suppressing further errors.", 7312233, false);
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
					}), false);
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
				Room room = RegionAndRoomQuery.RoomAtFast(start, this.map, RegionType.Set_Passable);
				if (room != null && room == RegionAndRoomQuery.RoomAtFast(dest.Cell, this.map, RegionType.Set_Passable))
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
				this.pathGrid = this.map.pathGrid;
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
					this.destRegions.RemoveDuplicates<Region>();
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

		// Token: 0x060011FF RID: 4607 RVA: 0x000C4318 File Offset: 0x000C2518
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

		// Token: 0x06001200 RID: 4608 RVA: 0x000C43CC File Offset: 0x000C25CC
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
					BoolUnknown boolUnknown = this.cache.CachedResultFor(this.startingRegions[i].Room, this.destRegions[j].Room, traverseParams);
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

		// Token: 0x06001201 RID: 4609 RVA: 0x000C4464 File Offset: 0x000C2664
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
									this.cache.AddCachedResult(this.startingRegions[k].Room, region2.Room, traverseParams, true);
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
					this.cache.AddCachedResult(this.startingRegions[l].Room, this.destRegions[m].Room, traverseParams, false);
				}
			}
			return false;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x000C45C8 File Offset: 0x000C27C8
		private bool CheckCellBasedReachability(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			IntVec3 foundCell = IntVec3.Invalid;
			Region[] directRegionGrid = this.regionGrid.DirectGrid;
			PathGrid pathGrid = this.map.pathGrid;
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
					Log.ErrorOnce("Do not use this method for non-cell based modes!", 938476762, false);
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
							this.cache.AddCachedResult(this.startingRegions[i].Room, validRegionAt.Room, traverseParams, true);
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
						this.cache.AddCachedResult(this.startingRegions[j].Room, this.destRegions[k].Room, traverseParams, false);
					}
				}
			}
			return false;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0001305A File Offset: 0x0001125A
		public bool CanReachColony(IntVec3 c)
		{
			return this.CanReachFactionBase(c, Faction.OfPlayer);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x000C4758 File Offset: 0x000C2958
		public bool CanReachFactionBase(IntVec3 c, Faction factionBaseFaction)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return this.CanReach(c, MapGenerator.PlayerStartSpot, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
			}
			if (!c.Walkable(this.map))
			{
				return false;
			}
			Faction faction = this.map.ParentFaction ?? Faction.OfPlayer;
			List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
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
			return this.CanReachBiggestMapEdgeRoom(c);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000C48A0 File Offset: 0x000C2AA0
		public bool CanReachBiggestMapEdgeRoom(IntVec3 c)
		{
			Room room = null;
			for (int i = 0; i < this.map.regionGrid.allRooms.Count; i++)
			{
				Room room2 = this.map.regionGrid.allRooms[i];
				if (room2.TouchesMapEdge && (room == null || room2.RegionCount > room.RegionCount))
				{
					room = room2;
				}
			}
			return room != null && this.CanReach(c, room.Regions[0].AnyCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x000C492C File Offset: 0x000C2B2C
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
					}), false);
					return false;
				}
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			if (region.Room.TouchesMapEdge)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			bool foundReg = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.Room.TouchesMapEdge)
				{
					foundReg = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			return foundReg;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000C4A30 File Offset: 0x000C2C30
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
					}), false);
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

		// Token: 0x06001208 RID: 4616 RVA: 0x00013068 File Offset: 0x00011268
		private bool CanUseCache(TraverseMode mode)
		{
			return mode != TraverseMode.PassAllDestroyableThingsNotWater && mode != TraverseMode.NoPassClosedDoorsOrWater;
		}

		// Token: 0x04000E77 RID: 3703
		private Map map;

		// Token: 0x04000E78 RID: 3704
		private Queue<Region> openQueue = new Queue<Region>();

		// Token: 0x04000E79 RID: 3705
		private List<Region> startingRegions = new List<Region>();

		// Token: 0x04000E7A RID: 3706
		private List<Region> destRegions = new List<Region>();

		// Token: 0x04000E7B RID: 3707
		private uint reachedIndex = 1U;

		// Token: 0x04000E7C RID: 3708
		private int numRegionsOpened;

		// Token: 0x04000E7D RID: 3709
		private bool working;

		// Token: 0x04000E7E RID: 3710
		private ReachabilityCache cache = new ReachabilityCache();

		// Token: 0x04000E7F RID: 3711
		private PathGrid pathGrid;

		// Token: 0x04000E80 RID: 3712
		private RegionGrid regionGrid;
	}
}
