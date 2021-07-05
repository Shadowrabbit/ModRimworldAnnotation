using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200178A RID: 6026
	public class WorldGenStep_Rivers : WorldGenStep
	{
		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x06008AF7 RID: 35575 RVA: 0x0031E198 File Offset: 0x0031C398
		public override int SeedPart
		{
			get
			{
				return 605014749;
			}
		}

		// Token: 0x06008AF8 RID: 35576 RVA: 0x0031E19F File Offset: 0x0031C39F
		public override void GenerateFresh(string seed)
		{
			this.GenerateRivers();
		}

		// Token: 0x06008AF9 RID: 35577 RVA: 0x0031E1A8 File Offset: 0x0031C3A8
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

		// Token: 0x06008AFA RID: 35578 RVA: 0x0031E258 File Offset: 0x0031C458
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

		// Token: 0x06008AFB RID: 35579 RVA: 0x0031E2B8 File Offset: 0x0031C4B8
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

		// Token: 0x06008AFC RID: 35580 RVA: 0x0031E354 File Offset: 0x0031C554
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

		// Token: 0x06008AFD RID: 35581 RVA: 0x0031E3DC File Offset: 0x0031C5DC
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

		// Token: 0x06008AFE RID: 35582 RVA: 0x0031E498 File Offset: 0x0031C698
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

		// Token: 0x06008AFF RID: 35583 RVA: 0x0031E630 File Offset: 0x0031C830
		public static float CalculateEvaporationConstant(float temperature)
		{
			return 0.61121f * Mathf.Exp((18.678f - temperature / 234.5f) * (temperature / (257.14f + temperature))) / (temperature + 273f);
		}

		// Token: 0x06008B00 RID: 35584 RVA: 0x0031E65C File Offset: 0x0031C85C
		public static float CalculateRiverSurfaceArea(float flow)
		{
			return Mathf.Pow(flow, 0.5f);
		}

		// Token: 0x06008B01 RID: 35585 RVA: 0x0031E669 File Offset: 0x0031C869
		public static float CalculateEvaporativeArea(float flow)
		{
			return WorldGenStep_Rivers.CalculateRiverSurfaceArea(flow) + 0f;
		}

		// Token: 0x06008B02 RID: 35586 RVA: 0x0031E677 File Offset: 0x0031C877
		public static float CalculateTotalEvaporation(float flow, float temperature)
		{
			return WorldGenStep_Rivers.CalculateEvaporationConstant(temperature) * WorldGenStep_Rivers.CalculateEvaporativeArea(flow) * 250f;
		}

		// Token: 0x0400587F RID: 22655
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

		// Token: 0x04005880 RID: 22656
		private const float HillinessSmallHillsElevation = 15f;

		// Token: 0x04005881 RID: 22657
		private const float HillinessLargeHillsElevation = 250f;

		// Token: 0x04005882 RID: 22658
		private const float HillinessMountainousElevation = 500f;

		// Token: 0x04005883 RID: 22659
		private const float HillinessImpassableElevation = 1000f;

		// Token: 0x04005884 RID: 22660
		private const float NonRiverEvaporation = 0f;

		// Token: 0x04005885 RID: 22661
		private const float EvaporationMultiple = 250f;
	}
}
