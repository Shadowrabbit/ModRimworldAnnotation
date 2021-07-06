using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E17 RID: 7703
	public class SketchResolver_FloorFill : SketchResolver
	{
		// Token: 0x0600A6AF RID: 42671 RVA: 0x00306238 File Offset: 0x00304438
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect cellRect = parms.rect ?? parms.sketch.OccupiedRect;
			TerrainDef terrainDef;
			TerrainDef terrainDef2;
			if (!SketchResolver_FloorFill.TryFindFloors(out terrainDef, out terrainDef2, parms))
			{
				return;
			}
			bool flag = parms.floorFillRoomsOnly ?? false;
			bool flag2 = parms.singleFloorType ?? false;
			if (flag)
			{
				SketchResolver_FloorFill.tmpWalls.Clear();
				for (int i = 0; i < parms.sketch.Things.Count; i++)
				{
					SketchThing sketchThing = parms.sketch.Things[i];
					if (sketchThing.def.passability == Traversability.Impassable && sketchThing.def.Fillage == FillCategory.Full)
					{
						foreach (IntVec3 item in sketchThing.OccupiedRect)
						{
							SketchResolver_FloorFill.tmpWalls.Add(item);
						}
					}
				}
				SketchResolver_FloorFill.tmpVisited.Clear();
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 intVec = enumerator.Current;
						if (!SketchResolver_FloorFill.tmpWalls.Contains(intVec))
						{
							this.FloorFillRoom_NewTmp(intVec, SketchResolver_FloorFill.tmpWalls, SketchResolver_FloorFill.tmpVisited, parms.sketch, terrainDef, terrainDef2, cellRect, flag2);
						}
					}
					return;
				}
			}
			bool[,] array = AbstractShapeGenerator.Generate(cellRect.Width, cellRect.Height, true, true, false, true, false, 0f);
			foreach (IntVec3 intVec2 in cellRect)
			{
				if (!parms.sketch.ThingsAt(intVec2).Any((SketchThing x) => x.def.Fillage == FillCategory.Full))
				{
					if (array[intVec2.x - cellRect.minX, intVec2.z - cellRect.minZ] || flag2)
					{
						parms.sketch.AddTerrain(terrainDef, intVec2, false);
					}
					else
					{
						parms.sketch.AddTerrain(terrainDef2, intVec2, false);
					}
				}
			}
		}

		// Token: 0x0600A6B0 RID: 42672 RVA: 0x003064B0 File Offset: 0x003046B0
		protected override bool CanResolveInt(ResolveParams parms)
		{
			TerrainDef terrainDef;
			TerrainDef terrainDef2;
			return SketchResolver_FloorFill.TryFindFloors(out terrainDef, out terrainDef2, parms);
		}

		// Token: 0x0600A6B1 RID: 42673 RVA: 0x003064CC File Offset: 0x003046CC
		private static bool TryFindFloors(out TerrainDef floor1, out TerrainDef floor2, ResolveParams parms)
		{
			Predicate<TerrainDef> validator = (TerrainDef x) => SketchGenUtility.IsFloorAllowed_NewTmp(x, parms.allowWood ?? true, parms.allowConcrete ?? true, parms.useOnlyStonesAvailableOnMap, parms.onlyBuildableByPlayer ?? false, parms.onlyStoneFloors ?? true);
			if (!BaseGenUtility.TryRandomInexpensiveFloor(out floor1, validator))
			{
				floor2 = null;
				return false;
			}
			if (parms.singleFloorType ?? false)
			{
				floor2 = null;
				return true;
			}
			TerrainDef floor1Local = floor1;
			return BaseGenUtility.TryRandomInexpensiveFloor(out floor2, (TerrainDef x) => x != floor1Local && (validator == null || validator(x)));
		}

		// Token: 0x0600A6B2 RID: 42674 RVA: 0x0030654C File Offset: 0x0030474C
		[Obsolete("Obsolete. Only needed for mod back-compatibility.")]
		private void FloorFillRoom(IntVec3 c, HashSet<IntVec3> walls, HashSet<IntVec3> visited, Sketch sketch, TerrainDef def1, TerrainDef def2, CellRect outerRect)
		{
			this.FloorFillRoom_NewTmp(c, walls, visited, sketch, def1, def2, outerRect, false);
		}

		// Token: 0x0600A6B3 RID: 42675 RVA: 0x0030656C File Offset: 0x0030476C
		private void FloorFillRoom_NewTmp(IntVec3 c, HashSet<IntVec3> walls, HashSet<IntVec3> visited, Sketch sketch, TerrainDef def1, TerrainDef def2, CellRect outerRect, bool singleFloorType)
		{
			if (visited.Contains(c))
			{
				return;
			}
			SketchResolver_FloorFill.tmpCells.Clear();
			SketchResolver_FloorFill.tmpStack.Clear();
			SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(c.x, c.z));
			visited.Add(c);
			int num = c.x;
			int num2 = c.x;
			int num3 = c.z;
			int num4 = c.z;
			while (SketchResolver_FloorFill.tmpStack.Count != 0)
			{
				Pair<int, int> pair = SketchResolver_FloorFill.tmpStack.Pop();
				int first = pair.First;
				int second = pair.Second;
				SketchResolver_FloorFill.tmpCells.Add(new IntVec3(first, 0, second));
				num = Mathf.Min(num, first);
				num2 = Mathf.Max(num2, first);
				num3 = Mathf.Min(num3, second);
				num4 = Mathf.Max(num4, second);
				if (first > outerRect.minX && !walls.Contains(new IntVec3(first - 1, 0, second)) && !visited.Contains(new IntVec3(first - 1, 0, second)))
				{
					visited.Add(new IntVec3(first - 1, 0, second));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first - 1, second));
				}
				if (second > outerRect.minZ && !walls.Contains(new IntVec3(first, 0, second - 1)) && !visited.Contains(new IntVec3(first, 0, second - 1)))
				{
					visited.Add(new IntVec3(first, 0, second - 1));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first, second - 1));
				}
				if (first < outerRect.maxX && !walls.Contains(new IntVec3(first + 1, 0, second)) && !visited.Contains(new IntVec3(first + 1, 0, second)))
				{
					visited.Add(new IntVec3(first + 1, 0, second));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first + 1, second));
				}
				if (second < outerRect.maxZ && !walls.Contains(new IntVec3(first, 0, second + 1)) && !visited.Contains(new IntVec3(first, 0, second + 1)))
				{
					visited.Add(new IntVec3(first, 0, second + 1));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first, second + 1));
				}
			}
			for (int i = 0; i < SketchResolver_FloorFill.tmpCells.Count; i++)
			{
				if (outerRect.IsOnEdge(SketchResolver_FloorFill.tmpCells[i]))
				{
					return;
				}
			}
			CellRect cellRect = CellRect.FromLimits(num, num3, num2, num4);
			bool[,] array = AbstractShapeGenerator.Generate(cellRect.Width, cellRect.Height, true, true, false, true, false, 0f);
			for (int j = 0; j < SketchResolver_FloorFill.tmpCells.Count; j++)
			{
				IntVec3 intVec = SketchResolver_FloorFill.tmpCells[j];
				if (!sketch.ThingsAt(intVec).Any((SketchThing x) => x.def.passability == Traversability.Impassable && x.def.Fillage == FillCategory.Full))
				{
					if (array[intVec.x - cellRect.minX, intVec.z - cellRect.minZ] || singleFloorType)
					{
						sketch.AddTerrain(def1, intVec, false);
					}
					else
					{
						sketch.AddTerrain(def2, intVec, false);
					}
				}
			}
		}

		// Token: 0x0400710F RID: 28943
		private static HashSet<IntVec3> tmpWalls = new HashSet<IntVec3>();

		// Token: 0x04007110 RID: 28944
		private static HashSet<IntVec3> tmpVisited = new HashSet<IntVec3>();

		// Token: 0x04007111 RID: 28945
		private static Stack<Pair<int, int>> tmpStack = new Stack<Pair<int, int>>();

		// Token: 0x04007112 RID: 28946
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
