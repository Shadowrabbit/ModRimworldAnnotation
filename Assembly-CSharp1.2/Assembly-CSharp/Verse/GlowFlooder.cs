using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025A RID: 602
	public class GlowFlooder
	{
		// Token: 0x06000F47 RID: 3911 RVA: 0x000B5FFC File Offset: 0x000B41FC
		public GlowFlooder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new GlowFlooder.GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new GlowFlooder.CompareGlowFlooderLightSquares(this.calcGrid));
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x000B6080 File Offset: 0x000B4280
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

		// Token: 0x06000F49 RID: 3913 RVA: 0x000B637C File Offset: 0x000B457C
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

		// Token: 0x06000F4A RID: 3914 RVA: 0x000B63F8 File Offset: 0x000B45F8
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

		// Token: 0x04000C7F RID: 3199
		private Map map;

		// Token: 0x04000C80 RID: 3200
		private GlowFlooder.GlowFloodCell[] calcGrid;

		// Token: 0x04000C81 RID: 3201
		private FastPriorityQueue<int> openSet;

		// Token: 0x04000C82 RID: 3202
		private uint statusUnseenValue;

		// Token: 0x04000C83 RID: 3203
		private uint statusOpenValue = 1U;

		// Token: 0x04000C84 RID: 3204
		private uint statusFinalizedValue = 2U;

		// Token: 0x04000C85 RID: 3205
		private int mapSizeX;

		// Token: 0x04000C86 RID: 3206
		private int mapSizeZ;

		// Token: 0x04000C87 RID: 3207
		private CompGlower glower;

		// Token: 0x04000C88 RID: 3208
		private CellIndices cellIndices;

		// Token: 0x04000C89 RID: 3209
		private Color32[] glowGrid;

		// Token: 0x04000C8A RID: 3210
		private float attenLinearSlope;

		// Token: 0x04000C8B RID: 3211
		private Thing[] blockers = new Thing[8];

		// Token: 0x04000C8C RID: 3212
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

		// Token: 0x0200025B RID: 603
		private struct GlowFloodCell
		{
			// Token: 0x04000C8D RID: 3213
			public int intDist;

			// Token: 0x04000C8E RID: 3214
			public uint status;
		}

		// Token: 0x0200025C RID: 604
		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			// Token: 0x06000F4C RID: 3916 RVA: 0x00011738 File Offset: 0x0000F938
			public CompareGlowFlooderLightSquares(GlowFlooder.GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			// Token: 0x06000F4D RID: 3917 RVA: 0x00011747 File Offset: 0x0000F947
			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}

			// Token: 0x04000C8F RID: 3215
			private GlowFlooder.GlowFloodCell[] grid;
		}
	}
}
