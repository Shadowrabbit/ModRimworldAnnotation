using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005FD RID: 1533
	public class PathFinder
	{
		// Token: 0x06002BFA RID: 11258 RVA: 0x0010567C File Offset: 0x0010387C
		public PathFinder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			int num = this.mapSizeX * this.mapSizeZ;
			if (PathFinder.calcGrid == null || PathFinder.calcGrid.Length < num)
			{
				PathFinder.calcGrid = new PathFinder.PathFinderNodeFast[num];
			}
			this.openList = new FastPriorityQueue<PathFinder.CostNode>(new PathFinder.CostNodeComparer());
			this.regionCostCalculator = new RegionCostCalculatorWrapper(map);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x0010570C File Offset: 0x0010390C
		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, Pawn pawn, PathEndMode peMode = PathEndMode.OnCell, PathFinderCostTuning tuning = null)
		{
			bool canBashDoors = false;
			bool canBashFences = false;
			if (((pawn != null) ? pawn.CurJob : null) != null)
			{
				if (pawn.CurJob.canBashDoors)
				{
					canBashDoors = true;
				}
				if (pawn.CurJob.canBashFences)
				{
					canBashFences = true;
				}
			}
			return this.FindPath(start, dest, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, canBashDoors, false, canBashFences), peMode, tuning);
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00105760 File Offset: 0x00103960
		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, TraverseParms traverseParms, PathEndMode peMode = PathEndMode.OnCell, PathFinderCostTuning tuning = null)
		{
			if (DebugSettings.pathThroughWalls)
			{
				traverseParms.mode = TraverseMode.PassAllDestroyableThings;
			}
			Pawn pawn = traverseParms.pawn;
			if (pawn != null && pawn.Map != this.map)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath for pawn which is spawned in another map. His map PathFinder should have been used, not this one. pawn=",
					pawn,
					" pawn.Map=",
					pawn.Map,
					" map=",
					this.map
				}));
				return PawnPath.NotFound;
			}
			if (!start.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid start ",
					start,
					", pawn= ",
					pawn
				}));
				return PawnPath.NotFound;
			}
			if (!dest.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid dest ",
					dest,
					", pawn= ",
					pawn
				}));
				return PawnPath.NotFound;
			}
			if (traverseParms.mode == TraverseMode.ByPawn)
			{
				if (!pawn.CanReach(dest, peMode, Danger.Deadly, traverseParms.canBashDoors, traverseParms.canBashFences, traverseParms.mode))
				{
					return PawnPath.NotFound;
				}
			}
			else if (!this.map.reachability.CanReach(start, dest, peMode, traverseParms))
			{
				return PawnPath.NotFound;
			}
			this.PfProfilerBeginSample(string.Concat(new object[]
			{
				"FindPath for ",
				pawn,
				" from ",
				start,
				" to ",
				dest,
				dest.HasThing ? (" at " + dest.Cell) : ""
			}));
			this.cellIndices = this.map.cellIndices;
			this.pathingContext = this.map.pathing.For(traverseParms);
			this.pathGrid = this.pathingContext.pathGrid;
			this.traverseParms = traverseParms;
			this.edificeGrid = this.map.edificeGrid.InnerArray;
			this.blueprintGrid = this.map.blueprintGrid.InnerArray;
			int x = dest.Cell.x;
			int z = dest.Cell.z;
			int num = this.cellIndices.CellToIndex(start);
			int num2 = this.cellIndices.CellToIndex(dest.Cell);
			ByteGrid byteGrid = traverseParms.alwaysUseAvoidGrid ? this.map.avoidGrid.Grid : ((pawn != null) ? pawn.GetAvoidGrid(true) : null);
			bool flag = traverseParms.mode == TraverseMode.PassAllDestroyableThings || traverseParms.mode == TraverseMode.PassAllDestroyableThingsNotWater;
			bool flag2 = traverseParms.mode != TraverseMode.NoPassClosedDoorsOrWater && traverseParms.mode != TraverseMode.PassAllDestroyableThingsNotWater;
			bool flag3 = !flag;
			CellRect cellRect = this.CalculateDestinationRect(dest, peMode);
			bool flag4 = cellRect.Width == 1 && cellRect.Height == 1;
			int[] array = this.pathGrid.pathGrid;
			TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
			EdificeGrid edificeGrid = this.map.edificeGrid;
			int num3 = 0;
			int num4 = 0;
			Area allowedArea = this.GetAllowedArea(pawn);
			BoolGrid lordWalkGrid = this.GetLordWalkGrid(pawn);
			bool flag5 = pawn != null && PawnUtility.ShouldCollideWithPawns(pawn);
			bool flag6 = !flag && start.GetRegion(this.map, RegionType.Set_Passable) != null && flag2;
			bool flag7 = !flag || !flag3;
			bool flag8 = false;
			bool flag9 = pawn != null && pawn.Drafted;
			int num5 = (pawn != null && pawn.IsColonist) ? 100000 : 2000;
			tuning = (tuning ?? PathFinderCostTuning.DefaultTuning);
			int costBlockedWallBase = tuning.costBlockedWallBase;
			float costBlockedWallExtraPerHitPoint = tuning.costBlockedWallExtraPerHitPoint;
			int costOffLordWalkGrid = tuning.costOffLordWalkGrid;
			int num6 = 0;
			int num7 = 0;
			float num8 = this.DetermineHeuristicStrength(pawn, start, dest);
			int num9;
			int num10;
			if (pawn != null)
			{
				num9 = pawn.TicksPerMoveCardinal;
				num10 = pawn.TicksPerMoveDiagonal;
			}
			else
			{
				num9 = 13;
				num10 = 18;
			}
			this.CalculateAndAddDisallowedCorners(peMode, cellRect);
			this.InitStatusesAndPushStartNode(ref num, start);
			for (;;)
			{
				this.PfProfilerBeginSample("Open cell");
				if (this.openList.Count <= 0)
				{
					break;
				}
				num6 += this.openList.Count;
				num7++;
				PathFinder.CostNode costNode = this.openList.Pop();
				num = costNode.index;
				if (costNode.cost != PathFinder.calcGrid[num].costNodeCost)
				{
					this.PfProfilerEndSample();
				}
				else if (PathFinder.calcGrid[num].status == PathFinder.statusClosedValue)
				{
					this.PfProfilerEndSample();
				}
				else
				{
					IntVec3 intVec = this.cellIndices.IndexToCell(num);
					int x2 = intVec.x;
					int z2 = intVec.z;
					if (flag4)
					{
						if (num == num2)
						{
							goto Block_29;
						}
					}
					else if (cellRect.Contains(intVec) && !this.disallowedCornerIndices.Contains(num))
					{
						goto Block_31;
					}
					if (num3 > 160000)
					{
						goto Block_32;
					}
					this.PfProfilerEndSample();
					this.PfProfilerBeginSample("Neighbor consideration");
					for (int i = 0; i < 8; i++)
					{
						uint num11 = (uint)(x2 + PathFinder.Directions[i]);
						uint num12 = (uint)(z2 + PathFinder.Directions[i + 8]);
						if ((ulong)num11 < (ulong)((long)this.mapSizeX) && (ulong)num12 < (ulong)((long)this.mapSizeZ))
						{
							int num13 = (int)num11;
							int num14 = (int)num12;
							int num15 = this.cellIndices.CellToIndex(num13, num14);
							if (PathFinder.calcGrid[num15].status != PathFinder.statusClosedValue || flag8)
							{
								int num16 = 0;
								bool flag10 = false;
								if (flag2 || !new IntVec3(num13, 0, num14).GetTerrain(this.map).HasTag("Water"))
								{
									if (!this.pathGrid.WalkableFast(num15))
									{
										if (!flag)
										{
											goto IL_BD7;
										}
										flag10 = true;
										num16 += costBlockedWallBase;
										Building building = edificeGrid[num15];
										if (building == null || !PathFinder.IsDestroyable(building))
										{
											goto IL_BD7;
										}
										num16 += (int)((float)building.HitPoints * costBlockedWallExtraPerHitPoint);
									}
									if (i > 3)
									{
										switch (i)
										{
										case 4:
											if (this.BlocksDiagonalMovement(num - this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											if (this.BlocksDiagonalMovement(num + 1))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											break;
										case 5:
											if (this.BlocksDiagonalMovement(num + this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											if (this.BlocksDiagonalMovement(num + 1))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											break;
										case 6:
											if (this.BlocksDiagonalMovement(num + this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											if (this.BlocksDiagonalMovement(num - 1))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											break;
										case 7:
											if (this.BlocksDiagonalMovement(num - this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											if (this.BlocksDiagonalMovement(num - 1))
											{
												if (flag7)
												{
													goto IL_BD7;
												}
												num16 += costBlockedWallBase;
											}
											break;
										}
									}
									int num17 = (i > 3) ? num10 : num9;
									num17 += num16;
									if (!flag10)
									{
										num17 += array[num15];
										if (flag9)
										{
											num17 += topGrid[num15].extraDraftedPerceivedPathCost;
										}
										else
										{
											num17 += topGrid[num15].extraNonDraftedPerceivedPathCost;
										}
									}
									if (byteGrid != null)
									{
										num17 += (int)(byteGrid[num15] * 8);
									}
									if (allowedArea != null && !allowedArea[num15])
									{
										num17 += 600;
									}
									if (flag5 && PawnUtility.AnyPawnBlockingPathAt(new IntVec3(num13, 0, num14), pawn, false, false, true))
									{
										num17 += 175;
									}
									Building building2 = this.edificeGrid[num15];
									if (building2 != null)
									{
										this.PfProfilerBeginSample("Edifices");
										int buildingCost = PathFinder.GetBuildingCost(building2, traverseParms, pawn, tuning);
										if (buildingCost == 2147483647)
										{
											this.PfProfilerEndSample();
											goto IL_BD7;
										}
										num17 += buildingCost;
										this.PfProfilerEndSample();
									}
									List<Blueprint> list = this.blueprintGrid[num15];
									if (list != null)
									{
										this.PfProfilerBeginSample("Blueprints");
										int num18 = 0;
										for (int j = 0; j < list.Count; j++)
										{
											num18 = Mathf.Max(num18, PathFinder.GetBlueprintCost(list[j], pawn));
										}
										if (num18 == 2147483647)
										{
											this.PfProfilerEndSample();
											goto IL_BD7;
										}
										num17 += num18;
										this.PfProfilerEndSample();
									}
									if (tuning.custom != null)
									{
										num17 += tuning.custom.CostOffset(intVec, new IntVec3(num13, 0, num14));
									}
									if (lordWalkGrid != null && !lordWalkGrid[new IntVec3(num13, 0, num14)])
									{
										num17 += costOffLordWalkGrid;
									}
									int num19 = num17 + PathFinder.calcGrid[num].knownCost;
									ushort status = PathFinder.calcGrid[num15].status;
									if (status == PathFinder.statusClosedValue || status == PathFinder.statusOpenValue)
									{
										int num20 = 0;
										if (status == PathFinder.statusClosedValue)
										{
											num20 = num9;
										}
										if (PathFinder.calcGrid[num15].knownCost <= num19 + num20)
										{
											goto IL_BD7;
										}
									}
									if (flag8)
									{
										PathFinder.calcGrid[num15].heuristicCost = Mathf.RoundToInt((float)this.regionCostCalculator.GetPathCostFromDestToRegion(num15) * PathFinder.RegionHeuristicWeightByNodesOpened.Evaluate((float)num4));
										if (PathFinder.calcGrid[num15].heuristicCost < 0)
										{
											Log.ErrorOnce(string.Concat(new object[]
											{
												"Heuristic cost overflow for ",
												pawn.ToStringSafe<Pawn>(),
												" pathing from ",
												start,
												" to ",
												dest,
												"."
											}), pawn.GetHashCode() ^ 193840009);
											PathFinder.calcGrid[num15].heuristicCost = 0;
										}
									}
									else if (status != PathFinder.statusClosedValue && status != PathFinder.statusOpenValue)
									{
										int dx = Math.Abs(num13 - x);
										int dz = Math.Abs(num14 - z);
										int num21 = GenMath.OctileDistance(dx, dz, num9, num10);
										PathFinder.calcGrid[num15].heuristicCost = Mathf.RoundToInt((float)num21 * num8);
									}
									int num22 = num19 + PathFinder.calcGrid[num15].heuristicCost;
									if (num22 < 0)
									{
										Log.ErrorOnce(string.Concat(new object[]
										{
											"Node cost overflow for ",
											pawn.ToStringSafe<Pawn>(),
											" pathing from ",
											start,
											" to ",
											dest,
											"."
										}), pawn.GetHashCode() ^ 87865822);
										num22 = 0;
									}
									PathFinder.calcGrid[num15].parentIndex = num;
									PathFinder.calcGrid[num15].knownCost = num19;
									PathFinder.calcGrid[num15].status = PathFinder.statusOpenValue;
									PathFinder.calcGrid[num15].costNodeCost = num22;
									num4++;
									this.openList.Push(new PathFinder.CostNode(num15, num22));
								}
							}
						}
						IL_BD7:;
					}
					this.PfProfilerEndSample();
					num3++;
					PathFinder.calcGrid[num].status = PathFinder.statusClosedValue;
					if (num4 >= num5 && flag6 && !flag8)
					{
						flag8 = true;
						this.regionCostCalculator.Init(cellRect, traverseParms, num9, num10, byteGrid, allowedArea, flag9, this.disallowedCornerIndices);
						this.InitStatusesAndPushStartNode(ref num, start);
						num4 = 0;
						num3 = 0;
					}
				}
			}
			string text = (pawn != null && pawn.CurJob != null) ? pawn.CurJob.ToString() : "null";
			string text2 = (pawn != null && pawn.Faction != null) ? pawn.Faction.ToString() : "null";
			Log.Warning(string.Concat(new object[]
			{
				pawn,
				" pathing from ",
				start,
				" to ",
				dest,
				" ran out of cells to process.\nJob:",
				text,
				"\nFaction: ",
				text2
			}));
			this.DebugDrawRichData();
			this.PfProfilerEndSample();
			this.PfProfilerEndSample();
			return PawnPath.NotFound;
			Block_29:
			this.PfProfilerEndSample();
			PawnPath result = this.FinalizedPath(num, flag8);
			this.PfProfilerEndSample();
			return result;
			Block_31:
			this.PfProfilerEndSample();
			PawnPath result2 = this.FinalizedPath(num, flag8);
			this.PfProfilerEndSample();
			return result2;
			Block_32:
			Log.Warning(string.Concat(new object[]
			{
				pawn,
				" pathing from ",
				start,
				" to ",
				dest,
				" hit search limit of ",
				160000,
				" cells."
			}));
			this.DebugDrawRichData();
			this.PfProfilerEndSample();
			this.PfProfilerEndSample();
			return PawnPath.NotFound;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x001063C0 File Offset: 0x001045C0
		public static int GetBuildingCost(Building b, TraverseParms traverseParms, Pawn pawn, PathFinderCostTuning tuning = null)
		{
			tuning = (tuning ?? PathFinderCostTuning.DefaultTuning);
			int costBlockedDoor = tuning.costBlockedDoor;
			float costBlockedDoorPerHitPoint = tuning.costBlockedDoorPerHitPoint;
			Building_Door building_Door = b as Building_Door;
			if (building_Door != null)
			{
				switch (traverseParms.mode)
				{
				case TraverseMode.ByPawn:
					if (!traverseParms.canBashDoors && building_Door.IsForbiddenToPass(pawn))
					{
						return int.MaxValue;
					}
					if (building_Door.PawnCanOpen(pawn) && !building_Door.FreePassage)
					{
						return building_Door.TicksToOpenNow;
					}
					if (building_Door.CanPhysicallyPass(pawn))
					{
						return 0;
					}
					if (traverseParms.canBashDoors)
					{
						return 300;
					}
					return int.MaxValue;
				case TraverseMode.PassDoors:
					if (pawn != null && building_Door.PawnCanOpen(pawn) && !building_Door.IsForbiddenToPass(pawn) && !building_Door.FreePassage)
					{
						return building_Door.TicksToOpenNow;
					}
					if ((pawn != null && building_Door.CanPhysicallyPass(pawn)) || building_Door.FreePassage)
					{
						return 0;
					}
					return 150;
				case TraverseMode.NoPassClosedDoors:
				case TraverseMode.NoPassClosedDoorsOrWater:
					if (building_Door.FreePassage)
					{
						return 0;
					}
					return int.MaxValue;
				case TraverseMode.PassAllDestroyableThings:
				case TraverseMode.PassAllDestroyableThingsNotWater:
					if (pawn != null && building_Door.PawnCanOpen(pawn) && !building_Door.IsForbiddenToPass(pawn) && !building_Door.FreePassage)
					{
						return building_Door.TicksToOpenNow;
					}
					if ((pawn != null && building_Door.CanPhysicallyPass(pawn)) || building_Door.FreePassage)
					{
						return 0;
					}
					return costBlockedDoor + (int)((float)building_Door.HitPoints * costBlockedDoorPerHitPoint);
				}
			}
			else if (b.def.IsFence && traverseParms.fenceBlocked)
			{
				switch (traverseParms.mode)
				{
				case TraverseMode.ByPawn:
					if (traverseParms.canBashFences)
					{
						return 300;
					}
					return int.MaxValue;
				case TraverseMode.PassDoors:
				case TraverseMode.NoPassClosedDoors:
				case TraverseMode.NoPassClosedDoorsOrWater:
					return 0;
				case TraverseMode.PassAllDestroyableThings:
				case TraverseMode.PassAllDestroyableThingsNotWater:
					return costBlockedDoor + (int)((float)b.HitPoints * costBlockedDoorPerHitPoint);
				}
			}
			else if (pawn != null)
			{
				return (int)b.PathFindCostFor(pawn);
			}
			return 0;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x0010656E File Offset: 0x0010476E
		public static int GetBlueprintCost(Blueprint b, Pawn pawn)
		{
			if (pawn != null)
			{
				return (int)b.PathFindCostFor(pawn);
			}
			return 0;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x0010657C File Offset: 0x0010477C
		public static bool IsDestroyable(Thing th)
		{
			return th.def.useHitPoints && th.def.destroyable;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00106598 File Offset: 0x00104798
		private bool BlocksDiagonalMovement(int index)
		{
			return PathFinder.BlocksDiagonalMovement(index, this.pathingContext, this.traverseParms.canBashFences);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x001065B1 File Offset: 0x001047B1
		public static bool BlocksDiagonalMovement(int x, int z, PathingContext pc, bool canBashFences)
		{
			return PathFinder.BlocksDiagonalMovement(pc.map.cellIndices.CellToIndex(x, z), pc, canBashFences);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x001065CC File Offset: 0x001047CC
		public static bool BlocksDiagonalMovement(int index, PathingContext pc, bool canBashFences)
		{
			if (!pc.pathGrid.WalkableFast(index))
			{
				return true;
			}
			Building building = pc.map.edificeGrid[index];
			if (building != null)
			{
				if (building is Building_Door)
				{
					return true;
				}
				if (canBashFences && building.def.IsFence)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x0010661B File Offset: 0x0010481B
		private void DebugFlash(IntVec3 c, float colorPct, string str)
		{
			PathFinder.DebugFlash(c, this.map, colorPct, str);
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x0010662B File Offset: 0x0010482B
		private static void DebugFlash(IntVec3 c, Map map, float colorPct, string str)
		{
			map.debugDrawer.FlashCell(c, colorPct, str, 50);
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x00106640 File Offset: 0x00104840
		private PawnPath FinalizedPath(int finalIndex, bool usedRegionHeuristics)
		{
			PawnPath emptyPawnPath = this.map.pawnPathPool.GetEmptyPawnPath();
			int num = finalIndex;
			for (;;)
			{
				int parentIndex = PathFinder.calcGrid[num].parentIndex;
				emptyPawnPath.AddNode(this.map.cellIndices.IndexToCell(num));
				if (num == parentIndex)
				{
					break;
				}
				num = parentIndex;
			}
			emptyPawnPath.SetupFound((float)PathFinder.calcGrid[finalIndex].knownCost, usedRegionHeuristics);
			return emptyPawnPath;
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x001066AC File Offset: 0x001048AC
		private void InitStatusesAndPushStartNode(ref int curIndex, IntVec3 start)
		{
			PathFinder.statusOpenValue += 2;
			PathFinder.statusClosedValue += 2;
			if (PathFinder.statusClosedValue >= 65435)
			{
				this.ResetStatuses();
			}
			curIndex = this.cellIndices.CellToIndex(start);
			PathFinder.calcGrid[curIndex].knownCost = 0;
			PathFinder.calcGrid[curIndex].heuristicCost = 0;
			PathFinder.calcGrid[curIndex].costNodeCost = 0;
			PathFinder.calcGrid[curIndex].parentIndex = curIndex;
			PathFinder.calcGrid[curIndex].status = PathFinder.statusOpenValue;
			this.openList.Clear();
			this.openList.Push(new PathFinder.CostNode(curIndex, 0));
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00106770 File Offset: 0x00104970
		private void ResetStatuses()
		{
			int num = PathFinder.calcGrid.Length;
			for (int i = 0; i < num; i++)
			{
				PathFinder.calcGrid[i].status = 0;
			}
			PathFinder.statusOpenValue = 1;
			PathFinder.statusClosedValue = 2;
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x0000313F File Offset: 0x0000133F
		[Conditional("PFPROFILE")]
		private void PfProfilerBeginSample(string s)
		{
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x0000313F File Offset: 0x0000133F
		[Conditional("PFPROFILE")]
		private void PfProfilerEndSample()
		{
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x001067B0 File Offset: 0x001049B0
		private void DebugDrawRichData()
		{
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x001067C0 File Offset: 0x001049C0
		private float DetermineHeuristicStrength(Pawn pawn, IntVec3 start, LocalTargetInfo dest)
		{
			if (pawn != null && pawn.RaceProps.Animal)
			{
				return 1.75f;
			}
			float lengthHorizontal = (start - dest.Cell).LengthHorizontal;
			return (float)Mathf.RoundToInt(PathFinder.NonRegionBasedHeuristicStrengthHuman_DistanceCurve.Evaluate(lengthHorizontal));
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x0010680C File Offset: 0x00104A0C
		private CellRect CalculateDestinationRect(LocalTargetInfo dest, PathEndMode peMode)
		{
			CellRect result;
			if (!dest.HasThing || peMode == PathEndMode.OnCell)
			{
				result = CellRect.SingleCell(dest.Cell);
			}
			else
			{
				result = dest.Thing.OccupiedRect();
			}
			if (peMode == PathEndMode.Touch)
			{
				result = result.ExpandedBy(1);
			}
			return result;
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x00106850 File Offset: 0x00104A50
		private Area GetAllowedArea(Pawn pawn)
		{
			if (pawn != null && pawn.playerSettings != null && !pawn.Drafted && ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				Area area = pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (area != null && area.TrueCount <= 0)
				{
					area = null;
				}
				return area;
			}
			return null;
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x00106896 File Offset: 0x00104A96
		private BoolGrid GetLordWalkGrid(Pawn pawn)
		{
			BreachingGrid breachingGrid = BreachingUtility.BreachingGridFor(pawn);
			if (breachingGrid == null)
			{
				return null;
			}
			return breachingGrid.WalkGrid;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x001068AC File Offset: 0x00104AAC
		private void CalculateAndAddDisallowedCorners(PathEndMode peMode, CellRect destinationRect)
		{
			this.disallowedCornerIndices.Clear();
			if (peMode == PathEndMode.Touch)
			{
				int minX = destinationRect.minX;
				int minZ = destinationRect.minZ;
				int maxX = destinationRect.maxX;
				int maxZ = destinationRect.maxZ;
				if (!this.IsCornerTouchAllowed(minX + 1, minZ + 1, minX + 1, minZ, minX, minZ + 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(minX, minZ));
				}
				if (!this.IsCornerTouchAllowed(minX + 1, maxZ - 1, minX + 1, maxZ, minX, maxZ - 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(minX, maxZ));
				}
				if (!this.IsCornerTouchAllowed(maxX - 1, maxZ - 1, maxX - 1, maxZ, maxX, maxZ - 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(maxX, maxZ));
				}
				if (!this.IsCornerTouchAllowed(maxX - 1, minZ + 1, maxX - 1, minZ, maxX, minZ + 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(maxX, minZ));
				}
			}
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x001069B3 File Offset: 0x00104BB3
		private bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z)
		{
			return TouchPathEndModeUtility.IsCornerTouchAllowed(cornerX, cornerZ, adjCardinal1X, adjCardinal1Z, adjCardinal2X, adjCardinal2Z, this.pathingContext);
		}

		// Token: 0x04001ABB RID: 6843
		private Map map;

		// Token: 0x04001ABC RID: 6844
		private FastPriorityQueue<PathFinder.CostNode> openList;

		// Token: 0x04001ABD RID: 6845
		private static PathFinder.PathFinderNodeFast[] calcGrid;

		// Token: 0x04001ABE RID: 6846
		private static ushort statusOpenValue = 1;

		// Token: 0x04001ABF RID: 6847
		private static ushort statusClosedValue = 2;

		// Token: 0x04001AC0 RID: 6848
		private RegionCostCalculatorWrapper regionCostCalculator;

		// Token: 0x04001AC1 RID: 6849
		private int mapSizeX;

		// Token: 0x04001AC2 RID: 6850
		private int mapSizeZ;

		// Token: 0x04001AC3 RID: 6851
		private PathGrid pathGrid;

		// Token: 0x04001AC4 RID: 6852
		private TraverseParms traverseParms;

		// Token: 0x04001AC5 RID: 6853
		private PathingContext pathingContext;

		// Token: 0x04001AC6 RID: 6854
		private Building[] edificeGrid;

		// Token: 0x04001AC7 RID: 6855
		private List<Blueprint>[] blueprintGrid;

		// Token: 0x04001AC8 RID: 6856
		private CellIndices cellIndices;

		// Token: 0x04001AC9 RID: 6857
		private List<int> disallowedCornerIndices = new List<int>(4);

		// Token: 0x04001ACA RID: 6858
		public const int DefaultMoveTicksCardinal = 13;

		// Token: 0x04001ACB RID: 6859
		private const int DefaultMoveTicksDiagonal = 18;

		// Token: 0x04001ACC RID: 6860
		private const int SearchLimit = 160000;

		// Token: 0x04001ACD RID: 6861
		private static readonly int[] Directions = new int[]
		{
			0,
			1,
			0,
			-1,
			1,
			1,
			-1,
			-1,
			-1,
			0,
			1,
			0,
			-1,
			1,
			1,
			-1
		};

		// Token: 0x04001ACE RID: 6862
		private const int Cost_DoorToBash = 300;

		// Token: 0x04001ACF RID: 6863
		private const int Cost_FenceToBash = 300;

		// Token: 0x04001AD0 RID: 6864
		public const int Cost_OutsideAllowedArea = 600;

		// Token: 0x04001AD1 RID: 6865
		private const int Cost_PawnCollision = 175;

		// Token: 0x04001AD2 RID: 6866
		private const int NodesToOpenBeforeRegionBasedPathing_NonColonist = 2000;

		// Token: 0x04001AD3 RID: 6867
		private const int NodesToOpenBeforeRegionBasedPathing_Colonist = 100000;

		// Token: 0x04001AD4 RID: 6868
		private const float NonRegionBasedHeuristicStrengthAnimal = 1.75f;

		// Token: 0x04001AD5 RID: 6869
		private static readonly SimpleCurve NonRegionBasedHeuristicStrengthHuman_DistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(40f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2.8f),
				true
			}
		};

		// Token: 0x04001AD6 RID: 6870
		private static readonly SimpleCurve RegionHeuristicWeightByNodesOpened = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(3500f, 1f),
				true
			},
			{
				new CurvePoint(4500f, 5f),
				true
			},
			{
				new CurvePoint(30000f, 50f),
				true
			},
			{
				new CurvePoint(100000f, 500f),
				true
			}
		};

		// Token: 0x02001DAE RID: 7598
		internal struct CostNode
		{
			// Token: 0x0600AB61 RID: 43873 RVA: 0x00390F49 File Offset: 0x0038F149
			public CostNode(int index, int cost)
			{
				this.index = index;
				this.cost = cost;
			}

			// Token: 0x0400720C RID: 29196
			public int index;

			// Token: 0x0400720D RID: 29197
			public int cost;
		}

		// Token: 0x02001DAF RID: 7599
		private struct PathFinderNodeFast
		{
			// Token: 0x0400720E RID: 29198
			public int knownCost;

			// Token: 0x0400720F RID: 29199
			public int heuristicCost;

			// Token: 0x04007210 RID: 29200
			public int parentIndex;

			// Token: 0x04007211 RID: 29201
			public int costNodeCost;

			// Token: 0x04007212 RID: 29202
			public ushort status;
		}

		// Token: 0x02001DB0 RID: 7600
		internal class CostNodeComparer : IComparer<PathFinder.CostNode>
		{
			// Token: 0x0600AB62 RID: 43874 RVA: 0x00390F59 File Offset: 0x0038F159
			public int Compare(PathFinder.CostNode a, PathFinder.CostNode b)
			{
				return a.cost.CompareTo(b.cost);
			}
		}
	}
}
