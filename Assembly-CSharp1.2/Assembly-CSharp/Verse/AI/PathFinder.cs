using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A51 RID: 2641
	public class PathFinder
	{
		// Token: 0x06003EAF RID: 16047 RVA: 0x001791A8 File Offset: 0x001773A8
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

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00179238 File Offset: 0x00177438
		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, Pawn pawn, PathEndMode peMode = PathEndMode.OnCell)
		{
			bool canBash = false;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.canBash)
			{
				canBash = true;
			}
			return this.FindPath(start, dest, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, canBash), peMode);
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00179274 File Offset: 0x00177474
		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, TraverseParms traverseParms, PathEndMode peMode = PathEndMode.OnCell)
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
				}), false);
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
				}), false);
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
				}), false);
				return PawnPath.NotFound;
			}
			if (traverseParms.mode == TraverseMode.ByPawn)
			{
				if (!pawn.CanReach(dest, peMode, Danger.Deadly, traverseParms.canBash, traverseParms.mode))
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
			this.pathGrid = this.map.pathGrid;
			this.edificeGrid = this.map.edificeGrid.InnerArray;
			this.blueprintGrid = this.map.blueprintGrid.InnerArray;
			int x = dest.Cell.x;
			int z = dest.Cell.z;
			int num = this.cellIndices.CellToIndex(start);
			int num2 = this.cellIndices.CellToIndex(dest.Cell);
			ByteGrid byteGrid = (pawn != null) ? pawn.GetAvoidGrid(true) : null;
			bool flag = traverseParms.mode == TraverseMode.PassAllDestroyableThings || traverseParms.mode == TraverseMode.PassAllDestroyableThingsNotWater;
			bool flag2 = traverseParms.mode != TraverseMode.NoPassClosedDoorsOrWater && traverseParms.mode != TraverseMode.PassAllDestroyableThingsNotWater;
			bool flag3 = !flag;
			CellRect cellRect = this.CalculateDestinationRect(dest, peMode);
			bool flag4 = cellRect.Width == 1 && cellRect.Height == 1;
			int[] array = this.map.pathGrid.pathGrid;
			TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
			EdificeGrid edificeGrid = this.map.edificeGrid;
			int num3 = 0;
			int num4 = 0;
			Area allowedArea = this.GetAllowedArea(pawn);
			bool flag5 = pawn != null && PawnUtility.ShouldCollideWithPawns(pawn);
			bool flag6 = !flag && start.GetRegion(this.map, RegionType.Set_Passable) != null && flag2;
			bool flag7 = !flag || !flag3;
			bool flag8 = false;
			bool flag9 = pawn != null && pawn.Drafted;
			int num5 = (pawn != null && pawn.IsColonist) ? 100000 : 2000;
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
			this.CalculateAndAddDisallowedCorners(traverseParms, peMode, cellRect);
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
							goto Block_27;
						}
					}
					else if (cellRect.Contains(intVec) && !this.disallowedCornerIndices.Contains(num))
					{
						goto Block_29;
					}
					if (num3 > 160000)
					{
						goto Block_30;
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
											goto IL_B31;
										}
										flag10 = true;
										num16 += 70;
										Building building = edificeGrid[num15];
										if (building == null || !PathFinder.IsDestroyable(building))
										{
											goto IL_B31;
										}
										num16 += (int)((float)building.HitPoints * 0.2f);
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
													goto IL_B31;
												}
												num16 += 70;
											}
											if (this.BlocksDiagonalMovement(num + 1))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
											}
											break;
										case 5:
											if (this.BlocksDiagonalMovement(num + this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
											}
											if (this.BlocksDiagonalMovement(num + 1))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
											}
											break;
										case 6:
											if (this.BlocksDiagonalMovement(num + this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
											}
											if (this.BlocksDiagonalMovement(num - 1))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
											}
											break;
										case 7:
											if (this.BlocksDiagonalMovement(num - this.mapSizeX))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
											}
											if (this.BlocksDiagonalMovement(num - 1))
											{
												if (flag7)
												{
													goto IL_B31;
												}
												num16 += 70;
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
										int buildingCost = PathFinder.GetBuildingCost(building2, traverseParms, pawn);
										if (buildingCost == 2147483647)
										{
											this.PfProfilerEndSample();
											goto IL_B31;
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
											goto IL_B31;
										}
										num17 += num18;
										this.PfProfilerEndSample();
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
											goto IL_B31;
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
											}), pawn.GetHashCode() ^ 193840009, false);
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
										}), pawn.GetHashCode() ^ 87865822, false);
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
						IL_B31:;
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
			}), false);
			this.DebugDrawRichData();
			this.PfProfilerEndSample();
			this.PfProfilerEndSample();
			return PawnPath.NotFound;
			Block_27:
			this.PfProfilerEndSample();
			PawnPath result = this.FinalizedPath(num, flag8);
			this.PfProfilerEndSample();
			return result;
			Block_29:
			this.PfProfilerEndSample();
			PawnPath result2 = this.FinalizedPath(num, flag8);
			this.PfProfilerEndSample();
			return result2;
			Block_30:
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
			}), false);
			this.DebugDrawRichData();
			this.PfProfilerEndSample();
			this.PfProfilerEndSample();
			return PawnPath.NotFound;
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x00179E30 File Offset: 0x00178030
		public static int GetBuildingCost(Building b, TraverseParms traverseParms, Pawn pawn)
		{
			Building_Door building_Door = b as Building_Door;
			if (building_Door != null)
			{
				switch (traverseParms.mode)
				{
				case TraverseMode.ByPawn:
					if (!traverseParms.canBash && building_Door.IsForbiddenToPass(pawn))
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
					if (traverseParms.canBash)
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
					return 50 + (int)((float)building_Door.HitPoints * 0.2f);
				}
			}
			else if (pawn != null)
			{
				return (int)b.PathFindCostFor(pawn);
			}
			return 0;
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0002F102 File Offset: 0x0002D302
		public static int GetBlueprintCost(Blueprint b, Pawn pawn)
		{
			if (pawn != null)
			{
				return (int)b.PathFindCostFor(pawn);
			}
			return 0;
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0002F110 File Offset: 0x0002D310
		public static bool IsDestroyable(Thing th)
		{
			return th.def.useHitPoints && th.def.destroyable;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0002F12C File Offset: 0x0002D32C
		private bool BlocksDiagonalMovement(int x, int z)
		{
			return PathFinder.BlocksDiagonalMovement(x, z, this.map);
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0002F13B File Offset: 0x0002D33B
		private bool BlocksDiagonalMovement(int index)
		{
			return PathFinder.BlocksDiagonalMovement(index, this.map);
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0002F149 File Offset: 0x0002D349
		public static bool BlocksDiagonalMovement(int x, int z, Map map)
		{
			return PathFinder.BlocksDiagonalMovement(map.cellIndices.CellToIndex(x, z), map);
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x0002F15E File Offset: 0x0002D35E
		public static bool BlocksDiagonalMovement(int index, Map map)
		{
			return !map.pathGrid.WalkableFast(index) || map.edificeGrid[index] is Building_Door;
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0002F186 File Offset: 0x0002D386
		private void DebugFlash(IntVec3 c, float colorPct, string str)
		{
			PathFinder.DebugFlash(c, this.map, colorPct, str);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0002F196 File Offset: 0x0002D396
		private static void DebugFlash(IntVec3 c, Map map, float colorPct, string str)
		{
			map.debugDrawer.FlashCell(c, colorPct, str, 50);
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00179F6C File Offset: 0x0017816C
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

		// Token: 0x06003EBC RID: 16060 RVA: 0x00179FD8 File Offset: 0x001781D8
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

		// Token: 0x06003EBD RID: 16061 RVA: 0x0017A09C File Offset: 0x0017829C
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

		// Token: 0x06003EBE RID: 16062 RVA: 0x00006A05 File Offset: 0x00004C05
		[Conditional("PFPROFILE")]
		private void PfProfilerBeginSample(string s)
		{
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x00006A05 File Offset: 0x00004C05
		[Conditional("PFPROFILE")]
		private void PfProfilerEndSample()
		{
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x0017A0DC File Offset: 0x001782DC
		private void DebugDrawRichData()
		{
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0017A0EC File Offset: 0x001782EC
		private float DetermineHeuristicStrength(Pawn pawn, IntVec3 start, LocalTargetInfo dest)
		{
			if (pawn != null && pawn.RaceProps.Animal)
			{
				return 1.75f;
			}
			float lengthHorizontal = (start - dest.Cell).LengthHorizontal;
			return (float)Mathf.RoundToInt(PathFinder.NonRegionBasedHeuristicStrengthHuman_DistanceCurve.Evaluate(lengthHorizontal));
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0017A138 File Offset: 0x00178338
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

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0017A17C File Offset: 0x0017837C
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

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0017A1C4 File Offset: 0x001783C4
		private void CalculateAndAddDisallowedCorners(TraverseParms traverseParms, PathEndMode peMode, CellRect destinationRect)
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

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0002F1A8 File Offset: 0x0002D3A8
		private bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z)
		{
			return TouchPathEndModeUtility.IsCornerTouchAllowed(cornerX, cornerZ, adjCardinal1X, adjCardinal1Z, adjCardinal2X, adjCardinal2Z, this.map);
		}

		// Token: 0x04002B19 RID: 11033
		private Map map;

		// Token: 0x04002B1A RID: 11034
		private FastPriorityQueue<PathFinder.CostNode> openList;

		// Token: 0x04002B1B RID: 11035
		private static PathFinder.PathFinderNodeFast[] calcGrid;

		// Token: 0x04002B1C RID: 11036
		private static ushort statusOpenValue = 1;

		// Token: 0x04002B1D RID: 11037
		private static ushort statusClosedValue = 2;

		// Token: 0x04002B1E RID: 11038
		private RegionCostCalculatorWrapper regionCostCalculator;

		// Token: 0x04002B1F RID: 11039
		private int mapSizeX;

		// Token: 0x04002B20 RID: 11040
		private int mapSizeZ;

		// Token: 0x04002B21 RID: 11041
		private PathGrid pathGrid;

		// Token: 0x04002B22 RID: 11042
		private Building[] edificeGrid;

		// Token: 0x04002B23 RID: 11043
		private List<Blueprint>[] blueprintGrid;

		// Token: 0x04002B24 RID: 11044
		private CellIndices cellIndices;

		// Token: 0x04002B25 RID: 11045
		private List<int> disallowedCornerIndices = new List<int>(4);

		// Token: 0x04002B26 RID: 11046
		public const int DefaultMoveTicksCardinal = 13;

		// Token: 0x04002B27 RID: 11047
		private const int DefaultMoveTicksDiagonal = 18;

		// Token: 0x04002B28 RID: 11048
		private const int SearchLimit = 160000;

		// Token: 0x04002B29 RID: 11049
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

		// Token: 0x04002B2A RID: 11050
		private const int Cost_DoorToBash = 300;

		// Token: 0x04002B2B RID: 11051
		private const int Cost_BlockedWallBase = 70;

		// Token: 0x04002B2C RID: 11052
		private const float Cost_BlockedWallExtraPerHitPoint = 0.2f;

		// Token: 0x04002B2D RID: 11053
		private const int Cost_BlockedDoor = 50;

		// Token: 0x04002B2E RID: 11054
		private const float Cost_BlockedDoorPerHitPoint = 0.2f;

		// Token: 0x04002B2F RID: 11055
		public const int Cost_OutsideAllowedArea = 600;

		// Token: 0x04002B30 RID: 11056
		private const int Cost_PawnCollision = 175;

		// Token: 0x04002B31 RID: 11057
		private const int NodesToOpenBeforeRegionBasedPathing_NonColonist = 2000;

		// Token: 0x04002B32 RID: 11058
		private const int NodesToOpenBeforeRegionBasedPathing_Colonist = 100000;

		// Token: 0x04002B33 RID: 11059
		private const float NonRegionBasedHeuristicStrengthAnimal = 1.75f;

		// Token: 0x04002B34 RID: 11060
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

		// Token: 0x04002B35 RID: 11061
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

		// Token: 0x02000A52 RID: 2642
		internal struct CostNode
		{
			// Token: 0x06003EC7 RID: 16071 RVA: 0x0002F1BE File Offset: 0x0002D3BE
			public CostNode(int index, int cost)
			{
				this.index = index;
				this.cost = cost;
			}

			// Token: 0x04002B36 RID: 11062
			public int index;

			// Token: 0x04002B37 RID: 11063
			public int cost;
		}

		// Token: 0x02000A53 RID: 2643
		private struct PathFinderNodeFast
		{
			// Token: 0x04002B38 RID: 11064
			public int knownCost;

			// Token: 0x04002B39 RID: 11065
			public int heuristicCost;

			// Token: 0x04002B3A RID: 11066
			public int parentIndex;

			// Token: 0x04002B3B RID: 11067
			public int costNodeCost;

			// Token: 0x04002B3C RID: 11068
			public ushort status;
		}

		// Token: 0x02000A54 RID: 2644
		internal class CostNodeComparer : IComparer<PathFinder.CostNode>
		{
			// Token: 0x06003EC8 RID: 16072 RVA: 0x0002F1CE File Offset: 0x0002D3CE
			public int Compare(PathFinder.CostNode a, PathFinder.CostNode b)
			{
				return a.cost.CompareTo(b.cost);
			}
		}
	}
}
