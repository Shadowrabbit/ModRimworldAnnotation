using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001330 RID: 4912
	public static class AbstractShapeGenerator
	{
		// Token: 0x06006A8F RID: 27279 RVA: 0x0020DD08 File Offset: 0x0020BF08
		public static bool[,] Generate(int width, int height, bool horizontalSymmetry, bool verticalSymmetry, bool allTruesMustBeConnected = false, bool allowEnclosedFalses = true, bool preferOutlines = false, float wipedCircleRadiusPct = 0f)
		{
			bool[,] array = new bool[width, height];
			int num = 0;
			do
			{
				AbstractShapeGenerator.GenerateInt(array, horizontalSymmetry, verticalSymmetry, allowEnclosedFalses, preferOutlines, wipedCircleRadiusPct);
				if (AbstractShapeGenerator.IsValid(array, allTruesMustBeConnected, allowEnclosedFalses, preferOutlines, wipedCircleRadiusPct))
				{
					return array;
				}
				num++;
			}
			while (num <= 500);
			Log.Error(string.Concat(new object[]
			{
				"AbstractShapeGenerator could not generate a valid shape after ",
				500,
				" tries. width=",
				width,
				" height=",
				height,
				" preferOutlines=",
				preferOutlines.ToString()
			}), false);
			return array;
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x0020DDA4 File Offset: 0x0020BFA4
		private static void GenerateInt(bool[,] grid, bool horizontalSymmetry, bool verticalSymmetry, bool allowEnclosedFalses, bool preferOutlines, float wipedCircleRadiusPct)
		{
			int length = grid.GetLength(0);
			int length2 = grid.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					grid[i, j] = false;
				}
			}
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			if (preferOutlines)
			{
				num = 0;
				num2 = 2;
				num3 = 0;
				num4 = 4;
				num5 = 2;
				if (length >= 16 || length2 >= 16)
				{
					num6 = 4;
				}
				else if (length >= 13 || length2 >= 13)
				{
					num6 = 3;
				}
				else if (length >= 12 || length2 >= 12)
				{
					num6 = 2;
				}
				else
				{
					num6 = 1;
				}
			}
			else
			{
				num = Rand.RangeInclusive(1, 3);
				num2 = 0;
				num3 = Rand.RangeInclusive(1, 3);
				num6 = 0;
				num4 = 0;
				num5 = 0;
			}
			float num7 = 0.3f;
			float num8 = 0.3f;
			float num9 = 0.7f;
			float num10 = 0.7f;
			for (int k = 0; k < num; k++)
			{
				foreach (IntVec3 intVec in CellRect.CenteredOn(new IntVec3(Rand.RangeInclusive(0, length - 1), 0, Rand.RangeInclusive(0, length2 - 1)), Mathf.Max(Mathf.RoundToInt((float)Rand.RangeInclusive(1, length) * num7), 1), Mathf.Max(Mathf.RoundToInt((float)Rand.RangeInclusive(1, length2) * num7), 1)))
				{
					if (intVec.x >= 0 && intVec.x < length && intVec.z >= 0 && intVec.z < length2)
					{
						grid[intVec.x, intVec.z] = true;
					}
				}
			}
			for (int l = 0; l < num2; l++)
			{
				CellRect cellRect = CellRect.CenteredOn(new IntVec3(Rand.RangeInclusive(0, length - 1), 0, Rand.RangeInclusive(0, length2 - 1)), Mathf.Max(Mathf.RoundToInt((float)Rand.RangeInclusive(1, length) * num8), 1), Mathf.Max(Mathf.RoundToInt((float)Rand.RangeInclusive(1, length2) * num8), 1));
				Rot4 random = Rot4.Random;
				foreach (IntVec3 intVec2 in cellRect.EdgeCells)
				{
					if ((allowEnclosedFalses || !cellRect.IsOnEdge(intVec2, random) || cellRect.IsOnEdge(intVec2, random.Rotated(RotationDirection.Clockwise)) || cellRect.IsOnEdge(intVec2, random.Rotated(RotationDirection.Counterclockwise))) && intVec2.x >= 0 && intVec2.x < length && intVec2.z >= 0 && intVec2.z < length2)
					{
						grid[intVec2.x, intVec2.z] = true;
					}
				}
			}
			for (int m = 0; m < num3; m++)
			{
				IntVec2 intVec3 = new IntVec2(Rand.RangeInclusive(0, length - 1), Rand.RangeInclusive(0, length2 - 1));
				foreach (IntVec3 intVec4 in GenRadial.RadialPatternInRadius((float)Mathf.Max(Mathf.RoundToInt((float)(Mathf.Max(length, length2) / 2) * num9), 1)))
				{
					intVec4.x += intVec3.x;
					intVec4.z += intVec3.z;
					if (intVec4.x >= 0 && intVec4.x < length && intVec4.z >= 0 && intVec4.z < length2)
					{
						grid[intVec4.x, intVec4.z] = true;
					}
				}
			}
			for (int n = 0; n < num6; n++)
			{
				float num11 = Rand.Range(0.7f, 1f);
				IntVec2 intVec5 = new IntVec2(Rand.RangeInclusive(0, length - 1), Rand.RangeInclusive(0, length2 - 1));
				int num12 = Mathf.Max(Mathf.RoundToInt((float)(Mathf.Max(length, length2) / 2) * num10 * num11), 1);
				bool @bool = Rand.Bool;
				AbstractShapeGenerator.tmpCircleCells.Clear();
				AbstractShapeGenerator.tmpCircleCells.AddRange(GenRadial.RadialPatternInRadius((float)num12));
				foreach (IntVec3 intVec6 in AbstractShapeGenerator.tmpCircleCells)
				{
					if ((allowEnclosedFalses || ((!@bool || intVec6.x >= 0) && (@bool || intVec6.z >= 0))) && (!AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x - 1, 0, intVec6.z - 1)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x - 1, 0, intVec6.z)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x - 1, 0, intVec6.z + 1)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x, 0, intVec6.z - 1)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x, 0, intVec6.z)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x, 0, intVec6.z + 1)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x + 1, 0, intVec6.z - 1)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x + 1, 0, intVec6.z)) || !AbstractShapeGenerator.tmpCircleCells.Contains(new IntVec3(intVec6.x + 1, 0, intVec6.z + 1))))
					{
						IntVec3 intVec7 = intVec6;
						intVec7.x += intVec5.x;
						intVec7.z += intVec5.z;
						if (intVec7.x >= 0 && intVec7.x < length && intVec7.z >= 0 && intVec7.z < length2)
						{
							grid[intVec7.x, intVec7.z] = true;
						}
					}
				}
			}
			for (int num13 = 0; num13 < num4; num13++)
			{
				bool bool2 = Rand.Bool;
				foreach (IntVec3 intVec8 in CellRect.CenteredOn(new IntVec3(Rand.RangeInclusive(0, length - 1), 0, Rand.RangeInclusive(0, length2 - 1)), bool2 ? Mathf.RoundToInt((float)Rand.RangeInclusive(1, length)) : 1, (!bool2) ? Mathf.RoundToInt((float)Rand.RangeInclusive(1, length2)) : 1))
				{
					if (intVec8.x >= 0 && intVec8.x < length && intVec8.z >= 0 && intVec8.z < length2)
					{
						grid[intVec8.x, intVec8.z] = true;
					}
				}
			}
			for (int num14 = 0; num14 < num5; num14++)
			{
				bool bool3 = Rand.Bool;
				CellRect cellRect2 = CellRect.CenteredOn(new IntVec3(Rand.RangeInclusive(0, length - 1), 0, Rand.RangeInclusive(0, length2 - 1)), Mathf.RoundToInt((float)Rand.RangeInclusive(1, length)), 1);
				foreach (IntVec3 intVec9 in cellRect2)
				{
					int num15 = intVec9.x - cellRect2.minX - cellRect2.Width / 2;
					if (bool3)
					{
						num15 = -num15;
					}
					IntVec3 intVec10 = intVec9;
					intVec10.z += num15;
					if (intVec10.x >= 0 && intVec10.x < length && intVec10.z >= 0 && intVec10.z < length2)
					{
						grid[intVec10.x, intVec10.z] = true;
					}
				}
			}
			if (preferOutlines)
			{
				for (int num16 = 0; num16 < grid.GetLength(0) - 1; num16++)
				{
					for (int num17 = 0; num17 < grid.GetLength(1) - 1; num17++)
					{
						if (grid[num16, num17] && grid[num16 + 1, num17] && grid[num16, num17 + 1] && grid[num16 + 1, num17 + 1])
						{
							int num18 = Rand.Range(0, 4);
							if (num18 == 0)
							{
								grid[num16, num17] = false;
							}
							else if (num18 == 1)
							{
								grid[num16 + 1, num17] = false;
							}
							else if (num18 == 2)
							{
								grid[num16, num17 + 1] = false;
							}
							else
							{
								grid[num16 + 1, num17 + 1] = false;
							}
						}
					}
				}
			}
			if (wipedCircleRadiusPct > 0f)
			{
				IntVec2 intVec11 = new IntVec2(length / 2, length2 / 2);
				foreach (IntVec3 intVec12 in GenRadial.RadialPatternInRadius((float)Mathf.FloorToInt((float)Mathf.Min(length, length2) * wipedCircleRadiusPct)))
				{
					intVec12.x += intVec11.x;
					intVec12.z += intVec11.z;
					if (intVec12.x >= 0 && intVec12.x < length && intVec12.z >= 0 && intVec12.z < length2)
					{
						grid[intVec12.x, intVec12.z] = false;
					}
				}
			}
			if (horizontalSymmetry)
			{
				for (int num19 = grid.GetLength(0) / 2; num19 < grid.GetLength(0); num19++)
				{
					for (int num20 = 0; num20 < grid.GetLength(1); num20++)
					{
						grid[num19, num20] = grid[grid.GetLength(0) - num19 - 1, num20];
					}
				}
			}
			if (verticalSymmetry)
			{
				for (int num21 = 0; num21 < grid.GetLength(0); num21++)
				{
					for (int num22 = grid.GetLength(1) / 2; num22 < grid.GetLength(1); num22++)
					{
						grid[num21, num22] = grid[num21, grid.GetLength(1) - num22 - 1];
					}
				}
			}
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x0020E838 File Offset: 0x0020CA38
		private static bool IsValid(bool[,] grid, bool allTruesMustBeConnected, bool allowEnclosedFalses, bool preferOutlines, float wipedCircleRadiusPct)
		{
			int num = 0;
			int upperBound = grid.GetUpperBound(0);
			int upperBound2 = grid.GetUpperBound(1);
			for (int i = grid.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = grid.GetLowerBound(1); j <= upperBound2; j++)
				{
					if (grid[i, j])
					{
						num++;
					}
				}
			}
			if (grid.GetLength(0) >= 3 && grid.GetLength(1) >= 3)
			{
				float num2 = 1f;
				if (wipedCircleRadiusPct > 0f)
				{
					int num3 = Mathf.FloorToInt((float)Mathf.Min(grid.GetLength(0), grid.GetLength(1)) * wipedCircleRadiusPct);
					float num4 = 3.1415927f * (float)num3 * (float)num3;
					num2 = 1f - Mathf.Clamp01(num4 / (float)(grid.GetLength(0) * grid.GetLength(1)));
				}
				int num5 = grid.GetLength(0) * grid.GetLength(1);
				int num6 = Mathf.FloorToInt((float)num5 * (preferOutlines ? 0.24f : 0.6f) * num2);
				int num7 = Mathf.CeilToInt((float)num5 * (preferOutlines ? 0.53f : 0.85f) * num2);
				if (num < num6 || num > num7)
				{
					return false;
				}
			}
			if (grid.GetLength(0) >= 2 && grid.GetLength(1) >= 2)
			{
				bool flag = false;
				bool flag2 = false;
				for (int k = 0; k < grid.GetLength(0) - 1; k++)
				{
					for (int l = 0; l < grid.GetLength(1) - 1; l++)
					{
						if (grid[k, l] && grid[k + 1, l])
						{
							flag2 = true;
						}
						if (grid[k, l] && grid[k, l + 1])
						{
							flag = true;
						}
						if (flag2 && flag)
						{
							break;
						}
					}
				}
				if (!flag2 || !flag)
				{
					return false;
				}
			}
			if (allTruesMustBeConnected)
			{
				bool[,] array = new bool[grid.GetLength(0), grid.GetLength(1)];
				bool flag3 = false;
				for (int m = 0; m < grid.GetLength(0); m++)
				{
					for (int n = 0; n < grid.GetLength(1); n++)
					{
						if (grid[m, n] && !array[m, n])
						{
							if (flag3)
							{
								return false;
							}
							flag3 = true;
							AbstractShapeGenerator.MarkVisited(m, n, grid, array, false);
						}
					}
				}
			}
			if (!allowEnclosedFalses)
			{
				bool[,] array2 = new bool[grid.GetLength(0), grid.GetLength(1)];
				for (int num8 = 0; num8 < grid.GetLength(0); num8++)
				{
					if (!grid[num8, 0])
					{
						AbstractShapeGenerator.MarkVisited(num8, 0, grid, array2, true);
					}
					if (!grid[num8, grid.GetLength(1) - 1])
					{
						AbstractShapeGenerator.MarkVisited(num8, grid.GetLength(1) - 1, grid, array2, true);
					}
				}
				for (int num9 = 0; num9 < grid.GetLength(1); num9++)
				{
					if (!grid[0, num9])
					{
						AbstractShapeGenerator.MarkVisited(0, num9, grid, array2, true);
					}
					if (!grid[grid.GetLength(0) - 1, num9])
					{
						AbstractShapeGenerator.MarkVisited(grid.GetLength(0) - 1, num9, grid, array2, true);
					}
				}
				for (int num10 = 0; num10 < grid.GetLength(0); num10++)
				{
					for (int num11 = 0; num11 < grid.GetLength(1); num11++)
					{
						if (!grid[num10, num11] && !array2[num10, num11])
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x0020EB7C File Offset: 0x0020CD7C
		private static void MarkVisited(int startX, int startY, bool[,] grid, bool[,] visited, bool traverseFalses)
		{
			if (visited[startX, startY])
			{
				return;
			}
			AbstractShapeGenerator.tmpStack.Clear();
			AbstractShapeGenerator.tmpStack.Push(new Pair<int, int>(startX, startY));
			visited[startX, startY] = true;
			while (AbstractShapeGenerator.tmpStack.Count != 0)
			{
				Pair<int, int> pair = AbstractShapeGenerator.tmpStack.Pop();
				int first = pair.First;
				int second = pair.Second;
				if (first > 0 && grid[first - 1, second] == !traverseFalses && !visited[first - 1, second])
				{
					visited[first - 1, second] = true;
					AbstractShapeGenerator.tmpStack.Push(new Pair<int, int>(first - 1, second));
				}
				if (second > 0 && grid[first, second - 1] == !traverseFalses && !visited[first, second - 1])
				{
					visited[first, second - 1] = true;
					AbstractShapeGenerator.tmpStack.Push(new Pair<int, int>(first, second - 1));
				}
				if (first + 1 < grid.GetLength(0) && grid[first + 1, second] == !traverseFalses && !visited[first + 1, second])
				{
					visited[first + 1, second] = true;
					AbstractShapeGenerator.tmpStack.Push(new Pair<int, int>(first + 1, second));
				}
				if (second + 1 < grid.GetLength(1) && grid[first, second + 1] == !traverseFalses && !visited[first, second + 1])
				{
					visited[first, second + 1] = true;
					AbstractShapeGenerator.tmpStack.Push(new Pair<int, int>(first, second + 1));
				}
			}
		}

		// Token: 0x040046E0 RID: 18144
		private const int MaxIterations = 500;

		// Token: 0x040046E1 RID: 18145
		private const float MinTruesPct = 0.6f;

		// Token: 0x040046E2 RID: 18146
		private const float MaxTruesPct = 0.85f;

		// Token: 0x040046E3 RID: 18147
		private const float MinTruesPct_PreferOutlines = 0.24f;

		// Token: 0x040046E4 RID: 18148
		private const float MaxTruesPct_PreferOutlines = 0.53f;

		// Token: 0x040046E5 RID: 18149
		private static HashSet<IntVec3> tmpCircleCells = new HashSet<IntVec3>();

		// Token: 0x040046E6 RID: 18150
		private static Stack<Pair<int, int>> tmpStack = new Stack<Pair<int, int>>();
	}
}
