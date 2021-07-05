using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005FE RID: 1534
	public sealed class PathGrid
	{
		// Token: 0x06002C12 RID: 11282 RVA: 0x00106AAA File Offset: 0x00104CAA
		public PathGrid(Map map, bool fenceArePassable)
		{
			this.map = map;
			this.fenceArePassable = fenceArePassable;
			this.pathGrid = new int[this.map.cellIndices.NumGridCells];
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x00106ADB File Offset: 0x00104CDB
		public bool Walkable(IntVec3 loc)
		{
			return loc.InBounds(this.map) && this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x00106B0C File Offset: 0x00104D0C
		public bool WalkableFast(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x00106B2D File Offset: 0x00104D2D
		public bool WalkableFast(int x, int z)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(x, z)] < 10000;
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x00106B4F File Offset: 0x00104D4F
		public bool WalkableFast(int index)
		{
			return this.pathGrid[index] < 10000;
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x00106B60 File Offset: 0x00104D60
		public int PerceivedPathCostAt(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)];
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00106B7C File Offset: 0x00104D7C
		private void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			bool flag = false;
			this.RecalculatePerceivedPathCostAt(c, ref flag);
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x00106B94 File Offset: 0x00104D94
		public void RecalculatePerceivedPathCostAt(IntVec3 c, ref bool haveNotified)
		{
			if (!c.InBounds(this.map))
			{
				return;
			}
			bool flag = this.WalkableFast(c);
			this.pathGrid[this.map.cellIndices.CellToIndex(c)] = this.CalculatedCostAt(c, true, IntVec3.Invalid);
			if (!haveNotified)
			{
				bool flag2 = this.WalkableFast(c);
				if (flag2 != flag)
				{
					this.map.reachability.ClearCache();
					this.map.regionDirtyer.Notify_WalkabilityChanged(c, flag2);
					haveNotified = true;
				}
			}
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x00106C14 File Offset: 0x00104E14
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.RecalculatePerceivedPathCostAt(c);
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x00106C68 File Offset: 0x00104E68
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
				if (!this.fenceArePassable && thing.def.building != null && thing.def.building.isFence)
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

		// Token: 0x06002C1C RID: 11292 RVA: 0x00106E60 File Offset: 0x00105060
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

		// Token: 0x06002C1D RID: 11293 RVA: 0x00106EA6 File Offset: 0x001050A6
		private static bool IsPathCostIgnoreRepeater(ThingDef def)
		{
			return def.pathCost >= 25 && def.pathCostIgnoreRepeat;
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x00106EBC File Offset: 0x001050BC
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001AD7 RID: 6871
		private readonly Map map;

		// Token: 0x04001AD8 RID: 6872
		private readonly bool fenceArePassable;

		// Token: 0x04001AD9 RID: 6873
		public readonly int[] pathGrid;

		// Token: 0x04001ADA RID: 6874
		public const int ImpassableCost = 10000;
	}
}
