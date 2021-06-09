using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A55 RID: 2645
	public sealed class PathGrid
	{
		// Token: 0x06003ECA RID: 16074 RVA: 0x0002F1E2 File Offset: 0x0002D3E2
		public PathGrid(Map map)
		{
			this.map = map;
			this.ResetPathGrid();
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0002F1F7 File Offset: 0x0002D3F7
		public void ResetPathGrid()
		{
			this.pathGrid = new int[this.map.cellIndices.NumGridCells];
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x0002F214 File Offset: 0x0002D414
		public bool Walkable(IntVec3 loc)
		{
			return loc.InBounds(this.map) && this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0002F245 File Offset: 0x0002D445
		public bool WalkableFast(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0002F266 File Offset: 0x0002D466
		public bool WalkableFast(int x, int z)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(x, z)] < 10000;
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0002F288 File Offset: 0x0002D488
		public bool WalkableFast(int index)
		{
			return this.pathGrid[index] < 10000;
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0002F299 File Offset: 0x0002D499
		public int PerceivedPathCostAt(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)];
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0017A3AC File Offset: 0x001785AC
		public void RecalculatePerceivedPathCostUnderThing(Thing t)
		{
			if (t.def.size == IntVec2.One)
			{
				this.RecalculatePerceivedPathCostAt(t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0017A424 File Offset: 0x00178624
		public void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return;
			}
			bool flag = this.WalkableFast(c);
			this.pathGrid[this.map.cellIndices.CellToIndex(c)] = this.CalculatedCostAt(c, true, IntVec3.Invalid);
			if (this.WalkableFast(c) != flag)
			{
				this.map.reachability.ClearCache();
				this.map.regionDirtyer.Notify_WalkabilityChanged(c);
			}
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0017A498 File Offset: 0x00178698
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.RecalculatePerceivedPathCostAt(c);
			}
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0017A4EC File Offset: 0x001786EC
		public int CalculatedCostAt(IntVec3 c, bool perceivedStatic, IntVec3 prevCell)
		{
			bool flag = false;
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
			if (terrainDef == null || terrainDef.passability == Traversability.Impassable)
			{
				return 10000;
			}
			int num = terrainDef.pathCost;
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.passability == Traversability.Impassable)
				{
					return 10000;
				}
				if (!PathGrid.IsPathCostIgnoreRepeater(thing.def) || !prevCell.IsValid || !this.ContainsPathCostIgnoreRepeater(prevCell))
				{
					int pathCost = thing.def.pathCost;
					if (pathCost > num)
					{
						num = pathCost;
					}
				}
				if (thing is Building_Door && prevCell.IsValid)
				{
					Building edifice = prevCell.GetEdifice(this.map);
					if (edifice != null && edifice is Building_Door)
					{
						flag = true;
					}
				}
			}
			int num2 = SnowUtility.MovementTicksAddOn(this.map.snowGrid.GetCategory(c));
			if (num2 > num)
			{
				num = num2;
			}
			if (flag)
			{
				num += 45;
			}
			if (perceivedStatic)
			{
				for (int j = 0; j < 9; j++)
				{
					IntVec3 intVec = GenAdj.AdjacentCellsAndInside[j];
					IntVec3 c2 = c + intVec;
					if (c2.InBounds(this.map))
					{
						Fire fire = null;
						list = this.map.thingGrid.ThingsListAtFast(c2);
						for (int k = 0; k < list.Count; k++)
						{
							fire = (list[k] as Fire);
							if (fire != null)
							{
								break;
							}
						}
						if (fire != null && fire.parent == null)
						{
							if (intVec.x == 0 && intVec.z == 0)
							{
								num += 1000;
							}
							else
							{
								num += 150;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0017A6B8 File Offset: 0x001788B8
		private bool ContainsPathCostIgnoreRepeater(IntVec3 c)
		{
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(list[i].def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x0002F2B3 File Offset: 0x0002D4B3
		private static bool IsPathCostIgnoreRepeater(ThingDef def)
		{
			return def.pathCost >= 25 && def.pathCostIgnoreRepeat;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0017A700 File Offset: 0x00178900
		[DebugOutput]
		public static void ThingPathCostsIgnoreRepeaters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===============PATH COST IGNORE REPEATERS==============");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(thingDef) && thingDef.passability != Traversability.Impassable)
				{
					stringBuilder.AppendLine(thingDef.defName + " " + thingDef.pathCost);
				}
			}
			stringBuilder.AppendLine("===============NON-PATH COST IGNORE REPEATERS that are buildings with >0 pathCost ==============");
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (!PathGrid.IsPathCostIgnoreRepeater(thingDef2) && thingDef2.passability != Traversability.Impassable && thingDef2.category == ThingCategory.Building && thingDef2.pathCost > 0)
				{
					stringBuilder.AppendLine(thingDef2.defName + " " + thingDef2.pathCost);
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04002B3D RID: 11069
		private Map map;

		// Token: 0x04002B3E RID: 11070
		public int[] pathGrid;

		// Token: 0x04002B3F RID: 11071
		public const int ImpassableCost = 10000;
	}
}
