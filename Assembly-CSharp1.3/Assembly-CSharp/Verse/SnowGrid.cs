using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B5 RID: 437
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00042483 File Offset: 0x00040683
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0004248B File Offset: 0x0004068B
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00042494 File Offset: 0x00040694
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000424B9 File Offset: 0x000406B9
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000424E3 File Offset: 0x000406E3
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00042507 File Offset: 0x00040707
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00042514 File Offset: 0x00040714
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

		// Token: 0x06000C75 RID: 3189 RVA: 0x00042565 File Offset: 0x00040765
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0004257C File Offset: 0x0004077C
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

		// Token: 0x06000C77 RID: 3191 RVA: 0x00042624 File Offset: 0x00040824
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

		// Token: 0x06000C78 RID: 3192 RVA: 0x0004269C File Offset: 0x0004089C
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
					this.map.pathing.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00042737 File Offset: 0x00040937
		public float GetDepth(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00042765 File Offset: 0x00040965
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}

		// Token: 0x04000A05 RID: 2565
		private Map map;

		// Token: 0x04000A06 RID: 2566
		private float[] depthGrid;

		// Token: 0x04000A07 RID: 2567
		private double totalDepth;

		// Token: 0x04000A08 RID: 2568
		public const float MaxDepth = 1f;
	}
}
