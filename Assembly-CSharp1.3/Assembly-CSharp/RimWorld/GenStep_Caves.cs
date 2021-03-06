using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000C8D RID: 3213
	public class GenStep_Caves : GenStep
	{
		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x0018C087 File Offset: 0x0018A287
		public override int SeedPart
		{
			get
			{
				return 647814558;
			}
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0018C090 File Offset: 0x0018A290
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			this.directionNoise = new Perlin(0.002050000010058284, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> group = new List<IntVec3>();
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (!visited[intVec] && this.IsRock(intVec, elevation, map))
				{
					group.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => this.IsRock(x, elevation, map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							visited[x] = true;
							group.Add(x);
						});
					}
					floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
					this.Trim(group, map);
					this.RemoveSmallDisconnectedSubGroups(group, map);
					if (group.Count >= 300)
					{
						this.DoOpenTunnels(group, map);
						this.DoClosedTunnels(group, map);
					}
				}
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0018C248 File Offset: 0x0018A448
		private void Trim(List<IntVec3> group, Map map)
		{
			GenMorphology.Open(group, 6, map);
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x0018C252 File Offset: 0x0018A452
		private bool IsRock(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return c.InBounds(map) && elevation[c] > 0.7f;
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x0018C270 File Offset: 0x0018A470
		private void DoOpenTunnels(List<IntVec3> group, Map map)
		{
			int num = GenMath.RoundRandom((float)group.Count * Rand.Range(0.9f, 1.1f) * 5.8f / 10000f);
			num = Mathf.Min(num, 3);
			if (num > 0)
			{
				num = Rand.RangeInclusive(1, num);
			}
			float num2 = GenStep_Caves.TunnelsWidthPerRockCount.Evaluate((float)group.Count);
			for (int i = 0; i < num; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num3 = -1f;
				float dir = -1f;
				float num4 = -1f;
				for (int j = 0; j < 10; j++)
				{
					IntVec3 intVec = this.FindRandomEdgeCellForTunnel(group, map);
					float distToCave = this.GetDistToCave(intVec, group, map, 40f, false);
					float num6;
					float num5 = this.FindBestInitialDir(intVec, group, out num6);
					if (!start.IsValid || distToCave > num3 || (distToCave == num3 && num6 > num4))
					{
						start = intVec;
						num3 = distToCave;
						dir = num5;
						num4 = num6;
					}
				}
				float width = Rand.Range(num2 * 0.8f, num2);
				this.Dig(start, dir, width, group, map, false, null);
			}
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0018C37C File Offset: 0x0018A57C
		private void DoClosedTunnels(List<IntVec3> group, Map map)
		{
			int num = GenMath.RoundRandom((float)group.Count * Rand.Range(0.9f, 1.1f) * 2.5f / 10000f);
			num = Mathf.Min(num, 1);
			if (num > 0)
			{
				num = Rand.RangeInclusive(0, num);
			}
			float num2 = GenStep_Caves.TunnelsWidthPerRockCount.Evaluate((float)group.Count);
			for (int i = 0; i < num; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num3 = -1f;
				for (int j = 0; j < 7; j++)
				{
					IntVec3 intVec = group.RandomElement<IntVec3>();
					float distToCave = this.GetDistToCave(intVec, group, map, 30f, true);
					if (!start.IsValid || distToCave > num3)
					{
						start = intVec;
						num3 = distToCave;
					}
				}
				float width = Rand.Range(num2 * 0.8f, num2);
				this.Dig(start, Rand.Range(0f, 360f), width, group, map, true, null);
			}
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0018C45C File Offset: 0x0018A65C
		private IntVec3 FindRandomEdgeCellForTunnel(List<IntVec3> group, Map map)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			GenStep_Caves.tmpCells.Clear();
			GenStep_Caves.tmpGroupSet.Clear();
			GenStep_Caves.tmpGroupSet.AddRange(group);
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].DistanceToEdge(map) >= 3 && caves[group[i]] <= 0f)
				{
					for (int j = 0; j < 4; j++)
					{
						IntVec3 item = group[i] + cardinalDirections[j];
						if (!GenStep_Caves.tmpGroupSet.Contains(item))
						{
							GenStep_Caves.tmpCells.Add(group[i]);
							break;
						}
					}
				}
			}
			if (!GenStep_Caves.tmpCells.Any<IntVec3>())
			{
				Log.Warning("Could not find any valid edge cell.");
				return group.RandomElement<IntVec3>();
			}
			return GenStep_Caves.tmpCells.RandomElement<IntVec3>();
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0018C534 File Offset: 0x0018A734
		private float FindBestInitialDir(IntVec3 start, List<IntVec3> group, out float dist)
		{
			float num = (float)this.GetDistToNonRock(start, group, IntVec3.East, 40);
			float num2 = (float)this.GetDistToNonRock(start, group, IntVec3.West, 40);
			float num3 = (float)this.GetDistToNonRock(start, group, IntVec3.South, 40);
			float num4 = (float)this.GetDistToNonRock(start, group, IntVec3.North, 40);
			float num5 = (float)this.GetDistToNonRock(start, group, IntVec3.NorthWest, 40);
			float num6 = (float)this.GetDistToNonRock(start, group, IntVec3.NorthEast, 40);
			float num7 = (float)this.GetDistToNonRock(start, group, IntVec3.SouthWest, 40);
			float num8 = (float)this.GetDistToNonRock(start, group, IntVec3.SouthEast, 40);
			dist = Mathf.Max(new float[]
			{
				num,
				num2,
				num3,
				num4,
				num5,
				num6,
				num7,
				num8
			});
			return GenMath.MaxByRandomIfEqual<float>(0f, num + num8 / 2f + num6 / 2f, 45f, num8 + num3 / 2f + num / 2f, 90f, num3 + num8 / 2f + num7 / 2f, 135f, num7 + num3 / 2f + num2 / 2f, 180f, num2 + num7 / 2f + num5 / 2f, 225f, num5 + num4 / 2f + num2 / 2f, 270f, num4 + num6 / 2f + num5 / 2f, 315f, num6 + num4 / 2f + num / 2f, 0.0001f);
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0018C6C4 File Offset: 0x0018A8C4
		private void Dig(IntVec3 start, float dir, float width, List<IntVec3> group, Map map, bool closed, HashSet<IntVec3> visited = null)
		{
			Vector3 vector = start.ToVector3Shifted();
			IntVec3 intVec = start;
			float num = 0f;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			bool flag = false;
			bool flag2 = false;
			if (visited == null)
			{
				visited = new HashSet<IntVec3>();
			}
			GenStep_Caves.tmpGroupSet.Clear();
			GenStep_Caves.tmpGroupSet.AddRange(group);
			int num2 = 0;
			for (;;)
			{
				if (closed)
				{
					int num3 = GenRadial.NumCellsInRadius(width / 2f + 1.5f);
					for (int i = 0; i < num3; i++)
					{
						IntVec3 intVec2 = intVec + GenRadial.RadialPattern[i];
						if (!visited.Contains(intVec2) && (!GenStep_Caves.tmpGroupSet.Contains(intVec2) || caves[intVec2] > 0f))
						{
							return;
						}
					}
				}
				if (num2 >= 15 && width > 1.4f + GenStep_Caves.BranchedTunnelWidthOffset.max)
				{
					if (!flag && Rand.Chance(0.1f))
					{
						this.DigInBestDirection(intVec, dir, new FloatRange(40f, 90f), width - GenStep_Caves.BranchedTunnelWidthOffset.RandomInRange, group, map, closed, visited);
						flag = true;
					}
					if (!flag2 && Rand.Chance(0.1f))
					{
						this.DigInBestDirection(intVec, dir, new FloatRange(-90f, -40f), width - GenStep_Caves.BranchedTunnelWidthOffset.RandomInRange, group, map, closed, visited);
						flag2 = true;
					}
				}
				bool flag3;
				this.SetCaveAround(intVec, width, map, visited, out flag3);
				if (flag3)
				{
					return;
				}
				while (vector.ToIntVec3() == intVec)
				{
					vector += Vector3Utility.FromAngleFlat(dir) * 0.5f;
					num += 0.5f;
				}
				if (!GenStep_Caves.tmpGroupSet.Contains(vector.ToIntVec3()))
				{
					return;
				}
				IntVec3 intVec3 = new IntVec3(intVec.x, 0, vector.ToIntVec3().z);
				if (this.IsRock(intVec3, elevation, map))
				{
					caves[intVec3] = Mathf.Max(caves[intVec3], width);
					visited.Add(intVec3);
				}
				intVec = vector.ToIntVec3();
				dir += (float)this.directionNoise.GetValue((double)(num * 60f), (double)((float)start.x * 200f), (double)((float)start.z * 200f)) * 8f;
				width -= 0.034f;
				if (width < 1.4f)
				{
					return;
				}
				num2++;
			}
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0018C91C File Offset: 0x0018AB1C
		private void DigInBestDirection(IntVec3 curIntVec, float curDir, FloatRange dirOffset, float width, List<IntVec3> group, Map map, bool closed, HashSet<IntVec3> visited = null)
		{
			int num = -1;
			float dir = -1f;
			for (int i = 0; i < 6; i++)
			{
				float num2 = curDir + dirOffset.RandomInRange;
				int distToNonRock = this.GetDistToNonRock(curIntVec, group, num2, 50);
				if (distToNonRock > num)
				{
					num = distToNonRock;
					dir = num2;
				}
			}
			if (num < 18)
			{
				return;
			}
			this.Dig(curIntVec, dir, width, group, map, closed, visited);
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0018C978 File Offset: 0x0018AB78
		private void SetCaveAround(IntVec3 around, float tunnelWidth, Map map, HashSet<IntVec3> visited, out bool hitAnotherTunnel)
		{
			hitAnotherTunnel = false;
			int num = GenRadial.NumCellsInRadius(tunnelWidth / 2f);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = around + GenRadial.RadialPattern[i];
				if (this.IsRock(intVec, elevation, map))
				{
					if (caves[intVec] > 0f && !visited.Contains(intVec))
					{
						hitAnotherTunnel = true;
					}
					caves[intVec] = Mathf.Max(caves[intVec], tunnelWidth);
					visited.Add(intVec);
				}
			}
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0018CA0C File Offset: 0x0018AC0C
		private int GetDistToNonRock(IntVec3 from, List<IntVec3> group, IntVec3 offset, int maxDist)
		{
			GenStep_Caves.groupSet.Clear();
			GenStep_Caves.groupSet.AddRange(group);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = from + offset * i;
				if (!GenStep_Caves.groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0018CA5C File Offset: 0x0018AC5C
		private int GetDistToNonRock(IntVec3 from, List<IntVec3> group, float dir, int maxDist)
		{
			GenStep_Caves.groupSet.Clear();
			GenStep_Caves.groupSet.AddRange(group);
			Vector3 a = Vector3Utility.FromAngleFlat(dir);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = (from.ToVector3Shifted() + a * (float)i).ToIntVec3();
				if (!GenStep_Caves.groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0018CAC0 File Offset: 0x0018ACC0
		private float GetDistToCave(IntVec3 cell, List<IntVec3> group, Map map, float maxDist, bool treatOpenSpaceAsCave)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			GenStep_Caves.tmpGroupSet.Clear();
			GenStep_Caves.tmpGroupSet.AddRange(group);
			int num = GenRadial.NumCellsInRadius(maxDist);
			IntVec3[] radialPattern = GenRadial.RadialPattern;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + radialPattern[i];
				if ((treatOpenSpaceAsCave && !GenStep_Caves.tmpGroupSet.Contains(intVec)) || (intVec.InBounds(map) && caves[intVec] > 0f))
				{
					return cell.DistanceTo(intVec);
				}
			}
			return maxDist;
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x0018CB48 File Offset: 0x0018AD48
		private void RemoveSmallDisconnectedSubGroups(List<IntVec3> group, Map map)
		{
			GenStep_Caves.groupSet.Clear();
			GenStep_Caves.groupSet.AddRange(group);
			GenStep_Caves.groupVisited.Clear();
			for (int i = 0; i < group.Count; i++)
			{
				if (!GenStep_Caves.groupVisited.Contains(group[i]) && GenStep_Caves.groupSet.Contains(group[i]))
				{
					GenStep_Caves.subGroup.Clear();
					map.floodFiller.FloodFill(group[i], (IntVec3 x) => GenStep_Caves.groupSet.Contains(x), delegate(IntVec3 x)
					{
						GenStep_Caves.subGroup.Add(x);
						GenStep_Caves.groupVisited.Add(x);
					}, int.MaxValue, false, null);
					if (GenStep_Caves.subGroup.Count < 300 || (float)GenStep_Caves.subGroup.Count < 0.05f * (float)group.Count)
					{
						for (int j = 0; j < GenStep_Caves.subGroup.Count; j++)
						{
							GenStep_Caves.groupSet.Remove(GenStep_Caves.subGroup[j]);
						}
					}
				}
			}
			group.Clear();
			group.AddRange(GenStep_Caves.groupSet);
		}

		// Token: 0x04002D65 RID: 11621
		private ModuleBase directionNoise;

		// Token: 0x04002D66 RID: 11622
		private static HashSet<IntVec3> tmpGroupSet = new HashSet<IntVec3>();

		// Token: 0x04002D67 RID: 11623
		private const float OpenTunnelsPer10k = 5.8f;

		// Token: 0x04002D68 RID: 11624
		private const float ClosedTunnelsPer10k = 2.5f;

		// Token: 0x04002D69 RID: 11625
		private const int MaxOpenTunnelsPerRockGroup = 3;

		// Token: 0x04002D6A RID: 11626
		private const int MaxClosedTunnelsPerRockGroup = 1;

		// Token: 0x04002D6B RID: 11627
		private const float DirectionChangeSpeed = 8f;

		// Token: 0x04002D6C RID: 11628
		private const float DirectionNoiseFrequency = 0.00205f;

		// Token: 0x04002D6D RID: 11629
		private const int MinRocksToGenerateAnyTunnel = 300;

		// Token: 0x04002D6E RID: 11630
		private const int AllowBranchingAfterThisManyCells = 15;

		// Token: 0x04002D6F RID: 11631
		private const float MinTunnelWidth = 1.4f;

		// Token: 0x04002D70 RID: 11632
		private const float WidthOffsetPerCell = 0.034f;

		// Token: 0x04002D71 RID: 11633
		private const float BranchChance = 0.1f;

		// Token: 0x04002D72 RID: 11634
		private static readonly FloatRange BranchedTunnelWidthOffset = new FloatRange(0.2f, 0.4f);

		// Token: 0x04002D73 RID: 11635
		private static readonly SimpleCurve TunnelsWidthPerRockCount = new SimpleCurve
		{
			{
				new CurvePoint(100f, 2f),
				true
			},
			{
				new CurvePoint(300f, 4f),
				true
			},
			{
				new CurvePoint(3000f, 5.5f),
				true
			}
		};

		// Token: 0x04002D74 RID: 11636
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04002D75 RID: 11637
		private static HashSet<IntVec3> groupSet = new HashSet<IntVec3>();

		// Token: 0x04002D76 RID: 11638
		private static HashSet<IntVec3> groupVisited = new HashSet<IntVec3>();

		// Token: 0x04002D77 RID: 11639
		private static List<IntVec3> subGroup = new List<IntVec3>();
	}
}
