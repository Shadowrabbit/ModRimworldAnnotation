using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020A0 RID: 8352
	public class WorldGenStep_Rivers : WorldGenStep
	{
		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600B0FA RID: 45306 RVA: 0x0007300B File Offset: 0x0007120B
		public override int SeedPart
		{
			get
			{
				return 605014749;
			}
		}

		// Token: 0x0600B0FB RID: 45307 RVA: 0x00073012 File Offset: 0x00071212
		public override void GenerateFresh(string seed)
		{
			this.GenerateRivers();
		}

		// Token: 0x0600B0FC RID: 45308 RVA: 0x00335CF0 File Offset: 0x00333EF0
		private void GenerateRivers()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts();
			List<int> coastalWaterTiles = this.GetCoastalWaterTiles();
			if (!coastalWaterTiles.Any<int>())
			{
				return;
			}
			List<int> neighbors = new List<int>();
			List<int>[] array = Find.WorldPathFinder.FloodPathsWithCostForTree(coastalWaterTiles, delegate(int st, int ed)
			{
				Tile tile = Find.WorldGrid[ed];
				Tile tile2 = Find.WorldGrid[st];
				Find.WorldGrid.GetTileNeighbors(ed, neighbors);
				int num = neighbors[0];
				for (int j = 0; j < neighbors.Count; j++)
				{
					if (WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[neighbors[j]]) < WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[num]))
					{
						num = neighbors[j];
					}
				}
				float num2 = 1f;
				if (num != st)
				{
					num2 = 2f;
				}
				return Mathf.RoundToInt(num2 * WorldGenStep_Rivers.ElevationChangeCost.Evaluate(WorldGenStep_Rivers.GetImpliedElevation(tile2) - WorldGenStep_Rivers.GetImpliedElevation(tile)));
			}, (int tid) => Find.WorldGrid[tid].WaterCovered, null);
			float[] flow = new float[array.Length];
			for (int i = 0; i < coastalWaterTiles.Count; i++)
			{
				this.AccumulateFlow(flow, array, coastalWaterTiles[i]);
				this.CreateRivers(flow, array, coastalWaterTiles[i]);
			}
		}

		// Token: 0x0600B0FD RID: 45309 RVA: 0x00335DA0 File Offset: 0x00333FA0
		private static float GetImpliedElevation(Tile tile)
		{
			float num = 0f;
			if (tile.hilliness == Hilliness.SmallHills)
			{
				num = 15f;
			}
			else if (tile.hilliness == Hilliness.LargeHills)
			{
				num = 250f;
			}
			else if (tile.hilliness == Hilliness.Mountainous)
			{
				num = 500f;
			}
			else if (tile.hilliness == Hilliness.Impassable)
			{
				num = 1000f;
			}
			return tile.elevation + num;
		}

		// Token: 0x0600B0FE RID: 45310 RVA: 0x00335E00 File Offset: 0x00334000
		private List<int> GetCoastalWaterTiles()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
			{
				if (Find.WorldGrid[i].biome == BiomeDefOf.Ocean)
				{
					Find.WorldGrid.GetTileNeighbors(i, list2);
					bool flag = false;
					for (int j = 0; j < list2.Count; j++)
					{
						if (Find.WorldGrid[list2[j]].biome != BiomeDefOf.Ocean)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(i);
					}
				}
			}
			return list;
		}

		// Token: 0x0600B0FF RID: 45311 RVA: 0x00335E9C File Offset: 0x0033409C
		private void AccumulateFlow(float[] flow, List<int>[] riverPaths, int index)
		{
			Tile tile = Find.WorldGrid[index];
			flow[index] += tile.rainfall;
			if (riverPaths[index] != null)
			{
				for (int i = 0; i < riverPaths[index].Count; i++)
				{
					this.AccumulateFlow(flow, riverPaths, riverPaths[index][i]);
					flow[index] += flow[riverPaths[index][i]];
				}
			}
			flow[index] = Mathf.Max(0f, flow[index] - WorldGenStep_Rivers.CalculateTotalEvaporation(flow[index], tile.temperature));
		}

		// Token: 0x0600B100 RID: 45312 RVA: 0x00335F24 File Offset: 0x00334124
		private void CreateRivers(float[] flow, List<int>[] riverPaths, int index)
		{
			List<int> list = new List<int>();
			Find.WorldGrid.GetTileNeighbors(index, list);
			for (int i = 0; i < list.Count; i++)
			{
				float targetFlow = flow[list[i]];
				RiverDef riverDef = (from rd in DefDatabase<RiverDef>.AllDefs
				where rd.spawnFlowThreshold > 0 && (float)rd.spawnFlowThreshold <= targetFlow
				select rd).MaxByWithFallback((RiverDef rd) => rd.spawnFlowThreshold, null);
				if (riverDef != null && Rand.Value < riverDef.spawnChance)
				{
					Find.WorldGrid.OverlayRiver(index, list[i], riverDef);
					this.ExtendRiver(flow, riverPaths, list[i], riverDef);
				}
			}
		}

		// Token: 0x0600B101 RID: 45313 RVA: 0x00335FE0 File Offset: 0x003341E0
		private void ExtendRiver(float[] flow, List<int>[] riverPaths, int index, RiverDef incomingRiver)
		{
			if (riverPaths[index] == null)
			{
				return;
			}
			int bestOutput = riverPaths[index].MaxBy((int ni) => flow[ni]);
			RiverDef riverDef = incomingRiver;
			while (riverDef != null && (float)riverDef.degradeThreshold > flow[bestOutput])
			{
				riverDef = riverDef.degradeChild;
			}
			if (riverDef != null)
			{
				Find.WorldGrid.OverlayRiver(index, bestOutput, riverDef);
				this.ExtendRiver(flow, riverPaths, bestOutput, riverDef);
			}
			if (incomingRiver.branches != null)
			{
				IEnumerable<int> source = riverPaths[index];
				Func<int, bool> <>9__1;
				Func<int, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((int ni) => ni != bestOutput));
				}
				using (IEnumerator<int> enumerator = source.Where(predicate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int alternateRiver = enumerator.Current;
						RiverDef.Branch branch2 = (from branch in incomingRiver.branches
						where (float)branch.minFlow <= flow[alternateRiver]
						select branch).MaxByWithFallback((RiverDef.Branch branch) => branch.minFlow, null);
						if (branch2 != null && Rand.Value < branch2.chance)
						{
							Find.WorldGrid.OverlayRiver(index, alternateRiver, branch2.child);
							this.ExtendRiver(flow, riverPaths, alternateRiver, branch2.child);
						}
					}
				}
			}
		}

		// Token: 0x0600B102 RID: 45314 RVA: 0x0007301A File Offset: 0x0007121A
		public static float CalculateEvaporationConstant(float temperature)
		{
			return 0.61121f * Mathf.Exp((18.678f - temperature / 234.5f) * (temperature / (257.14f + temperature))) / (temperature + 273f);
		}

		// Token: 0x0600B103 RID: 45315 RVA: 0x00073046 File Offset: 0x00071246
		public static float CalculateRiverSurfaceArea(float flow)
		{
			return Mathf.Pow(flow, 0.5f);
		}

		// Token: 0x0600B104 RID: 45316 RVA: 0x00073053 File Offset: 0x00071253
		public static float CalculateEvaporativeArea(float flow)
		{
			return WorldGenStep_Rivers.CalculateRiverSurfaceArea(flow) + 0f;
		}

		// Token: 0x0600B105 RID: 45317 RVA: 0x00073061 File Offset: 0x00071261
		public static float CalculateTotalEvaporation(float flow, float temperature)
		{
			return WorldGenStep_Rivers.CalculateEvaporationConstant(temperature) * WorldGenStep_Rivers.CalculateEvaporativeArea(flow) * 250f;
		}

		// Token: 0x040079D9 RID: 31193
		private static readonly SimpleCurve ElevationChangeCost = new SimpleCurve
		{
			{
				new CurvePoint(-1000f, 50f),
				true
			},
			{
				new CurvePoint(-100f, 100f),
				true
			},
			{
				new CurvePoint(0f, 400f),
				true
			},
			{
				new CurvePoint(0f, 5000f),
				true
			},
			{
				new CurvePoint(100f, 50000f),
				true
			},
			{
				new CurvePoint(1000f, 50000f),
				true
			}
		};

		// Token: 0x040079DA RID: 31194
		private const float HillinessSmallHillsElevation = 15f;

		// Token: 0x040079DB RID: 31195
		private const float HillinessLargeHillsElevation = 250f;

		// Token: 0x040079DC RID: 31196
		private const float HillinessMountainousElevation = 500f;

		// Token: 0x040079DD RID: 31197
		private const float HillinessImpassableElevation = 1000f;

		// Token: 0x040079DE RID: 31198
		private const float NonRiverEvaporation = 0f;

		// Token: 0x040079DF RID: 31199
		private const float EvaporationMultiple = 250f;
	}
}
