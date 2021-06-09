using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200026C RID: 620
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x00011E78 File Offset: 0x00010078
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00011E80 File Offset: 0x00010080
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00011E89 File Offset: 0x00010089
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00011EAE File Offset: 0x000100AE
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00011ED8 File Offset: 0x000100D8
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00011EFC File Offset: 0x000100FC
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x000B7C64 File Offset: 0x000B5E64
		private bool CanHaveSnow(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SnowGrid.CanCoexistWithSnow(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || terrainDef.holdSnow;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00011F06 File Offset: 0x00010106
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x000B7CB8 File Offset: 0x000B5EB8
		public void AddDepth(IntVec3 c, float depthToAdd)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			float num2 = this.depthGrid[num];
			if (num2 <= 0f && depthToAdd < 0f)
			{
				return;
			}
			if (num2 >= 0.999f && depthToAdd > 1f)
			{
				return;
			}
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			float num3 = num2 + depthToAdd;
			num3 = Mathf.Clamp(num3, 0f, 1f);
			float num4 = num3 - num2;
			this.totalDepth += (double)num4;
			if (Mathf.Abs(num4) > 0.0001f)
			{
				this.depthGrid[num] = num3;
				this.CheckVisualOrPathCostChange(c, num2, num3);
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x000B7D60 File Offset: 0x000B5F60
		public void SetDepth(IntVec3 c, float newDepth)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			newDepth = Mathf.Clamp(newDepth, 0f, 1f);
			float num2 = this.depthGrid[num];
			this.depthGrid[num] = newDepth;
			float num3 = newDepth - num2;
			this.totalDepth += (double)num3;
			this.CheckVisualOrPathCostChange(c, num2, newDepth);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x000B7DD8 File Offset: 0x000B5FD8
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (!Mathf.Approximately(oldDepth, newDepth))
			{
				if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things, true, false);
				}
				else if (newDepth == 0f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
				}
				if (SnowUtility.GetSnowCategory(oldDepth) != SnowUtility.GetSnowCategory(newDepth))
				{
					this.map.pathGrid.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00011F1D File Offset: 0x0001011D
		public float GetDepth(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00011F4B File Offset: 0x0001014B
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}

		// Token: 0x04000CC8 RID: 3272
		private Map map;

		// Token: 0x04000CC9 RID: 3273
		private float[] depthGrid;

		// Token: 0x04000CCA RID: 3274
		private double totalDepth;

		// Token: 0x04000CCB RID: 3275
		public const float MaxDepth = 1f;
	}
}
