using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002009 RID: 8201
	public class WorldPathFinder
	{
		// Token: 0x0600ADBC RID: 44476 RVA: 0x00071181 File Offset: 0x0006F381
		public WorldPathFinder()
		{
			this.calcGrid = new WorldPathFinder.PathFinderNodeFast[Find.WorldGrid.TilesCount];
			this.openList = new FastPriorityQueue<WorldPathFinder.CostNode>(new WorldPathFinder.CostNodeComparer());
		}

		// Token: 0x0600ADBD RID: 44477 RVA: 0x00328EB8 File Offset: 0x003270B8
		public WorldPath FindPath(int startTile, int destTile, Caravan caravan, Func<float, bool> terminator = null)
		{
			if (startTile < 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid start tile ",
					startTile,
					", caravan= ",
					caravan
				}), false);
				return WorldPath.NotFound;
			}
			if (destTile < 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid dest tile ",
					destTile,
					", caravan= ",
					caravan
				}), false);
				return WorldPath.NotFound;
			}
			if (caravan != null)
			{
				if (!caravan.CanReach(destTile))
				{
					return WorldPath.NotFound;
				}
			}
			else if (!Find.WorldReachability.CanReach(startTile, destTile))
			{
				return WorldPath.NotFound;
			}
			World world = Find.World;
			WorldGrid grid = world.grid;
			List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
			Vector3 normalized = grid.GetTileCenter(destTile).normalized;
			float[] movementDifficulty = world.pathGrid.movementDifficulty;
			int num = 0;
			int num2 = (caravan != null) ? caravan.TicksPerMove : 3300;
			int num3 = this.CalculateHeuristicStrength(startTile, destTile);
			this.statusOpenValue += 2;
			this.statusClosedValue += 2;
			if (this.statusClosedValue >= 65435)
			{
				this.ResetStatuses();
			}
			this.calcGrid[startTile].knownCost = 0;
			this.calcGrid[startTile].heuristicCost = 0;
			this.calcGrid[startTile].costNodeCost = 0;
			this.calcGrid[startTile].parentTile = startTile;
			this.calcGrid[startTile].status = this.statusOpenValue;
			this.openList.Clear();
			this.openList.Push(new WorldPathFinder.CostNode(startTile, 0));
			while (this.openList.Count > 0)
			{
				WorldPathFinder.CostNode costNode = this.openList.Pop();
				if (costNode.cost == this.calcGrid[costNode.tile].costNodeCost)
				{
					int tile = costNode.tile;
					if (this.calcGrid[tile].status != this.statusClosedValue)
					{
						if (tile == destTile)
						{
							return this.FinalizedPath(tile);
						}
						if (num > 500000)
						{
							Log.Warning(string.Concat(new object[]
							{
								caravan,
								" pathing from ",
								startTile,
								" to ",
								destTile,
								" hit search limit of ",
								500000,
								" tiles."
							}), false);
							return WorldPath.NotFound;
						}
						int num4 = (tile + 1 < tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_offsets[tile + 1] : tileIDToNeighbors_values.Count;
						for (int i = tileIDToNeighbors_offsets[tile]; i < num4; i++)
						{
							int num5 = tileIDToNeighbors_values[i];
							if (this.calcGrid[num5].status != this.statusClosedValue && !world.Impassable(num5))
							{
								int num6 = (int)((float)num2 * movementDifficulty[num5] * grid.GetRoadMovementDifficultyMultiplier(tile, num5, null)) + this.calcGrid[tile].knownCost;
								ushort status = this.calcGrid[num5].status;
								if ((status != this.statusClosedValue && status != this.statusOpenValue) || this.calcGrid[num5].knownCost > num6)
								{
									Vector3 tileCenter = grid.GetTileCenter(num5);
									if (status != this.statusClosedValue && status != this.statusOpenValue)
									{
										float num7 = grid.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, normalized));
										this.calcGrid[num5].heuristicCost = Mathf.RoundToInt((float)num2 * num7 * (float)num3 * 0.5f);
									}
									int num8 = num6 + this.calcGrid[num5].heuristicCost;
									this.calcGrid[num5].parentTile = tile;
									this.calcGrid[num5].knownCost = num6;
									this.calcGrid[num5].status = this.statusOpenValue;
									this.calcGrid[num5].costNodeCost = num8;
									this.openList.Push(new WorldPathFinder.CostNode(num5, num8));
								}
							}
						}
						num++;
						this.calcGrid[tile].status = this.statusClosedValue;
						if (terminator != null && terminator((float)this.calcGrid[tile].costNodeCost))
						{
							return WorldPath.NotFound;
						}
					}
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				caravan,
				" pathing from ",
				startTile,
				" to ",
				destTile,
				" ran out of tiles to process."
			}), false);
			return WorldPath.NotFound;
		}

		// Token: 0x0600ADBE RID: 44478 RVA: 0x00329374 File Offset: 0x00327574
		public void FloodPathsWithCost(List<int> startTiles, Func<int, int, int> costFunc, Func<int, bool> impassable = null, Func<int, float, bool> terminator = null)
		{
			if (startTiles.Count < 1 || startTiles.Contains(-1))
			{
				Log.Error("Tried to FindPath with invalid start tiles", false);
				return;
			}
			World world = Find.World;
			WorldGrid grid = world.grid;
			List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
			if (impassable == null)
			{
				impassable = ((int tid) => world.Impassable(tid));
			}
			this.statusOpenValue += 2;
			this.statusClosedValue += 2;
			if (this.statusClosedValue >= 65435)
			{
				this.ResetStatuses();
			}
			this.openList.Clear();
			using (List<int>.Enumerator enumerator = startTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					this.calcGrid[num].knownCost = 0;
					this.calcGrid[num].costNodeCost = 0;
					this.calcGrid[num].parentTile = num;
					this.calcGrid[num].status = this.statusOpenValue;
					this.openList.Push(new WorldPathFinder.CostNode(num, 0));
				}
				goto IL_2F2;
			}
			IL_127:
			WorldPathFinder.CostNode costNode = this.openList.Pop();
			if (costNode.cost == this.calcGrid[costNode.tile].costNodeCost)
			{
				int tile = costNode.tile;
				if (this.calcGrid[tile].status != this.statusClosedValue)
				{
					int num2 = (tile + 1 < tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_offsets[tile + 1] : tileIDToNeighbors_values.Count;
					for (int i = tileIDToNeighbors_offsets[tile]; i < num2; i++)
					{
						int num3 = tileIDToNeighbors_values[i];
						if (this.calcGrid[num3].status != this.statusClosedValue && !impassable(num3))
						{
							int num4 = costFunc(tile, num3) + this.calcGrid[tile].knownCost;
							ushort status = this.calcGrid[num3].status;
							if ((status != this.statusClosedValue && status != this.statusOpenValue) || this.calcGrid[num3].knownCost > num4)
							{
								int num5 = num4;
								this.calcGrid[num3].parentTile = tile;
								this.calcGrid[num3].knownCost = num4;
								this.calcGrid[num3].status = this.statusOpenValue;
								this.calcGrid[num3].costNodeCost = num5;
								this.openList.Push(new WorldPathFinder.CostNode(num3, num5));
							}
						}
					}
					this.calcGrid[tile].status = this.statusClosedValue;
					if (terminator != null && terminator(tile, (float)this.calcGrid[tile].costNodeCost))
					{
						return;
					}
				}
			}
			IL_2F2:
			if (this.openList.Count > 0)
			{
				goto IL_127;
			}
		}

		// Token: 0x0600ADBF RID: 44479 RVA: 0x00329694 File Offset: 0x00327894
		public List<int>[] FloodPathsWithCostForTree(List<int> startTiles, Func<int, int, int> costFunc, Func<int, bool> impassable = null, Func<int, float, bool> terminator = null)
		{
			this.FloodPathsWithCost(startTiles, costFunc, impassable, terminator);
			WorldGrid grid = Find.World.grid;
			List<int>[] array = new List<int>[grid.TilesCount];
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (this.calcGrid[i].status == this.statusClosedValue)
				{
					int parentTile = this.calcGrid[i].parentTile;
					if (parentTile != i)
					{
						if (array[parentTile] == null)
						{
							array[parentTile] = new List<int>();
						}
						array[parentTile].Add(i);
					}
				}
			}
			return array;
		}

		// Token: 0x0600ADC0 RID: 44480 RVA: 0x0032971C File Offset: 0x0032791C
		private WorldPath FinalizedPath(int lastTile)
		{
			WorldPath emptyWorldPath = Find.WorldPathPool.GetEmptyWorldPath();
			int num = lastTile;
			for (;;)
			{
				int parentTile = this.calcGrid[num].parentTile;
				int num2 = num;
				emptyWorldPath.AddNodeAtStart(num2);
				if (num2 == parentTile)
				{
					break;
				}
				num = parentTile;
			}
			emptyWorldPath.SetupFound((float)this.calcGrid[lastTile].knownCost);
			return emptyWorldPath;
		}

		// Token: 0x0600ADC1 RID: 44481 RVA: 0x00329774 File Offset: 0x00327974
		private void ResetStatuses()
		{
			int num = this.calcGrid.Length;
			for (int i = 0; i < num; i++)
			{
				this.calcGrid[i].status = 0;
			}
			this.statusOpenValue = 1;
			this.statusClosedValue = 2;
		}

		// Token: 0x0600ADC2 RID: 44482 RVA: 0x003297B8 File Offset: 0x003279B8
		private int CalculateHeuristicStrength(int startTile, int destTile)
		{
			float x = Find.WorldGrid.ApproxDistanceInTiles(startTile, destTile);
			return Mathf.RoundToInt(WorldPathFinder.HeuristicStrength_DistanceCurve.Evaluate(x));
		}

		// Token: 0x04007752 RID: 30546
		private FastPriorityQueue<WorldPathFinder.CostNode> openList;

		// Token: 0x04007753 RID: 30547
		private WorldPathFinder.PathFinderNodeFast[] calcGrid;

		// Token: 0x04007754 RID: 30548
		private ushort statusOpenValue = 1;

		// Token: 0x04007755 RID: 30549
		private ushort statusClosedValue = 2;

		// Token: 0x04007756 RID: 30550
		private const int SearchLimit = 500000;

		// Token: 0x04007757 RID: 30551
		private static readonly SimpleCurve HeuristicStrength_DistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(30f, 1f),
				true
			},
			{
				new CurvePoint(40f, 1.3f),
				true
			},
			{
				new CurvePoint(130f, 2f),
				true
			}
		};

		// Token: 0x04007758 RID: 30552
		private const float BestRoadDiscount = 0.5f;

		// Token: 0x0200200A RID: 8202
		private struct CostNode
		{
			// Token: 0x0600ADC4 RID: 44484 RVA: 0x000711BC File Offset: 0x0006F3BC
			public CostNode(int tile, int cost)
			{
				this.tile = tile;
				this.cost = cost;
			}

			// Token: 0x04007759 RID: 30553
			public int tile;

			// Token: 0x0400775A RID: 30554
			public int cost;
		}

		// Token: 0x0200200B RID: 8203
		private struct PathFinderNodeFast
		{
			// Token: 0x0400775B RID: 30555
			public int knownCost;

			// Token: 0x0400775C RID: 30556
			public int heuristicCost;

			// Token: 0x0400775D RID: 30557
			public int parentTile;

			// Token: 0x0400775E RID: 30558
			public int costNodeCost;

			// Token: 0x0400775F RID: 30559
			public ushort status;
		}

		// Token: 0x0200200C RID: 8204
		private class CostNodeComparer : IComparer<WorldPathFinder.CostNode>
		{
			// Token: 0x0600ADC5 RID: 44485 RVA: 0x00329840 File Offset: 0x00327A40
			public int Compare(WorldPathFinder.CostNode a, WorldPathFinder.CostNode b)
			{
				int cost = a.cost;
				int cost2 = b.cost;
				if (cost > cost2)
				{
					return 1;
				}
				if (cost < cost2)
				{
					return -1;
				}
				return 0;
			}
		}
	}
}
