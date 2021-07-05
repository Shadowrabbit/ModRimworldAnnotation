using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A6 RID: 422
	public class GlowFlooder
	{
		// Token: 0x06000BD3 RID: 3027 RVA: 0x0004024C File Offset: 0x0003E44C
		public GlowFlooder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new GlowFlooder.GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new GlowFlooder.CompareGlowFlooderLightSquares(this.calcGrid));
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000402D0 File Offset: 0x0003E4D0
		public void AddFloodGlowFor(CompGlower theGlower, Color32[] glowGrid)
		{
			this.cellIndices = this.map.cellIndices;
			this.glowGrid = glowGrid;
			this.glower = theGlower;
			this.attenLinearSlope = -1f / theGlower.Props.glowRadius;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			IntVec3 position = theGlower.parent.Position;
			int num = Mathf.RoundToInt(this.glower.Props.glowRadius * 100f);
			int num2 = this.cellIndices.CellToIndex(position);
			this.InitStatusesAndPushStartNode(ref num2, position);
			while (this.openSet.Count != 0)
			{
				num2 = this.openSet.Pop();
				IntVec3 intVec = this.cellIndices.IndexToCell(num2);
				this.calcGrid[num2].status = this.statusFinalizedValue;
				this.SetGlowGridFromDist(num2);
				for (int i = 0; i < 8; i++)
				{
					uint num3 = (uint)(intVec.x + (int)GlowFlooder.Directions[i, 0]);
					uint num4 = (uint)(intVec.z + (int)GlowFlooder.Directions[i, 1]);
					if ((ulong)num3 < (ulong)((long)this.mapSizeX) && (ulong)num4 < (ulong)((long)this.mapSizeZ))
					{
						int x = (int)num3;
						int z = (int)num4;
						int num5 = this.cellIndices.CellToIndex(x, z);
						if (this.calcGrid[num5].status != this.statusFinalizedValue)
						{
							this.blockers[i] = innerArray[num5];
							if (this.blockers[i] != null)
							{
								if (this.blockers[i].def.blockLight)
								{
									goto IL_2DB;
								}
								this.blockers[i] = null;
							}
							int num6;
							if (i < 4)
							{
								num6 = 100;
							}
							else
							{
								num6 = 141;
							}
							int num7 = this.calcGrid[num2].intDist + num6;
							if (num7 <= num)
							{
								if (i >= 4)
								{
									switch (i)
									{
									case 4:
										if (this.blockers[0] != null && this.blockers[1] != null)
										{
											goto IL_2DB;
										}
										break;
									case 5:
										if (this.blockers[1] != null && this.blockers[2] != null)
										{
											goto IL_2DB;
										}
										break;
									case 6:
										if (this.blockers[2] != null && this.blockers[3] != null)
										{
											goto IL_2DB;
										}
										break;
									case 7:
										if (this.blockers[0] != null && this.blockers[3] != null)
										{
											goto IL_2DB;
										}
										break;
									}
								}
								if (this.calcGrid[num5].status <= this.statusUnseenValue)
								{
									this.calcGrid[num5].intDist = 999999;
									this.calcGrid[num5].status = this.statusOpenValue;
								}
								if (num7 < this.calcGrid[num5].intDist)
								{
									this.calcGrid[num5].intDist = num7;
									this.calcGrid[num5].status = this.statusOpenValue;
									this.openSet.Push(num5);
								}
							}
						}
					}
					IL_2DB:;
				}
			}
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000405CC File Offset: 0x0003E7CC
		private void InitStatusesAndPushStartNode(ref int curIndex, IntVec3 start)
		{
			this.statusUnseenValue += 3U;
			this.statusOpenValue += 3U;
			this.statusFinalizedValue += 3U;
			curIndex = this.cellIndices.CellToIndex(start);
			this.openSet.Clear();
			this.calcGrid[curIndex].intDist = 100;
			this.openSet.Clear();
			this.openSet.Push(curIndex);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00040648 File Offset: 0x0003E848
		private void SetGlowGridFromDist(int index)
		{
			float num = (float)this.calcGrid[index].intDist / 100f;
			ColorInt colorInt = default(ColorInt);
			if (num <= this.glower.Props.glowRadius)
			{
				float b = 1f / (num * num);
				float b2 = Mathf.Lerp(1f + this.attenLinearSlope * num, b, 0.4f);
				colorInt = this.glower.Props.glowColor * b2;
				colorInt.a = 0;
			}
			if (colorInt.r > 0 || colorInt.g > 0 || colorInt.b > 0)
			{
				colorInt.ClampToNonNegative();
				ColorInt colA = this.glowGrid[index].AsColorInt();
				colA += colorInt;
				if (num < this.glower.Props.overlightRadius)
				{
					colA.a = 1;
				}
				Color32 toColor = colA.ToColor32;
				this.glowGrid[index] = toColor;
			}
		}

		// Token: 0x040009C6 RID: 2502
		private Map map;

		// Token: 0x040009C7 RID: 2503
		private GlowFlooder.GlowFloodCell[] calcGrid;

		// Token: 0x040009C8 RID: 2504
		private FastPriorityQueue<int> openSet;

		// Token: 0x040009C9 RID: 2505
		private uint statusUnseenValue;

		// Token: 0x040009CA RID: 2506
		private uint statusOpenValue = 1U;

		// Token: 0x040009CB RID: 2507
		private uint statusFinalizedValue = 2U;

		// Token: 0x040009CC RID: 2508
		private int mapSizeX;

		// Token: 0x040009CD RID: 2509
		private int mapSizeZ;

		// Token: 0x040009CE RID: 2510
		private CompGlower glower;

		// Token: 0x040009CF RID: 2511
		private CellIndices cellIndices;

		// Token: 0x040009D0 RID: 2512
		private Color32[] glowGrid;

		// Token: 0x040009D1 RID: 2513
		private float attenLinearSlope;

		// Token: 0x040009D2 RID: 2514
		private Thing[] blockers = new Thing[8];

		// Token: 0x040009D3 RID: 2515
		private static readonly sbyte[,] Directions = new sbyte[,]
		{
			{
				0,
				-1
			},
			{
				1,
				0
			},
			{
				0,
				1
			},
			{
				-1,
				0
			},
			{
				1,
				-1
			},
			{
				1,
				1
			},
			{
				-1,
				1
			},
			{
				-1,
				-1
			}
		};

		// Token: 0x02001964 RID: 6500
		private struct GlowFloodCell
		{
			// Token: 0x04006170 RID: 24944
			public int intDist;

			// Token: 0x04006171 RID: 24945
			public uint status;
		}

		// Token: 0x02001965 RID: 6501
		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			// Token: 0x06009836 RID: 38966 RVA: 0x0035E739 File Offset: 0x0035C939
			public CompareGlowFlooderLightSquares(GlowFlooder.GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			// Token: 0x06009837 RID: 38967 RVA: 0x0035E748 File Offset: 0x0035C948
			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}

			// Token: 0x04006172 RID: 24946
			private GlowFlooder.GlowFloodCell[] grid;
		}
	}
}
