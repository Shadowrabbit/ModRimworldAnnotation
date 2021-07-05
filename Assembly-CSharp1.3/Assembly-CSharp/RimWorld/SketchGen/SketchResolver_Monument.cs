using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001592 RID: 5522
	public class SketchResolver_Monument : SketchResolver
	{
		// Token: 0x0600826B RID: 33387 RVA: 0x002E390C File Offset: 0x002E1B0C
		protected override void ResolveInt(ResolveParams parms)
		{
			SketchResolver_Monument.<>c__DisplayClass1_0 CS$<>8__locals1 = new SketchResolver_Monument.<>c__DisplayClass1_0();
			CS$<>8__locals1.parms = parms;
			CS$<>8__locals1.<>4__this = this;
			IntVec2 value;
			if (CS$<>8__locals1.parms.monumentSize != null)
			{
				value = CS$<>8__locals1.parms.monumentSize.Value;
			}
			else
			{
				int num = Rand.Range(10, 50);
				value = new IntVec2(num, num);
			}
			CS$<>8__locals1.width = value.x;
			CS$<>8__locals1.height = value.z;
			bool flag;
			if (CS$<>8__locals1.parms.monumentOpen != null)
			{
				flag = CS$<>8__locals1.parms.monumentOpen.Value;
			}
			else
			{
				flag = Rand.Chance(SketchResolver_Monument.OpenChancePerSizeCurve.Evaluate((float)Mathf.Max(CS$<>8__locals1.width, CS$<>8__locals1.height)));
			}
			CS$<>8__locals1.monument = new Sketch();
			CS$<>8__locals1.onlyBuildableByPlayer = (CS$<>8__locals1.parms.onlyBuildableByPlayer ?? false);
			CS$<>8__locals1.filterAllowsAll = (CS$<>8__locals1.parms.allowedMonumentThings == null);
			List<IntVec3> list = new List<IntVec3>();
			if (flag)
			{
				CS$<>8__locals1.horizontalSymmetry = true;
				CS$<>8__locals1.verticalSymmetry = true;
				bool[,] array = AbstractShapeGenerator.Generate(CS$<>8__locals1.width, CS$<>8__locals1.height, CS$<>8__locals1.horizontalSymmetry, CS$<>8__locals1.verticalSymmetry, false, false, true, 0f);
				for (int i = 0; i < array.GetLength(0); i++)
				{
					for (int j = 0; j < array.GetLength(1); j++)
					{
						if (array[i, j])
						{
							CS$<>8__locals1.monument.AddThing(ThingDefOf.Wall, new IntVec3(i, 0, j), Rot4.North, ThingDefOf.WoodLog, 1, null, null, true);
						}
					}
				}
			}
			else
			{
				CS$<>8__locals1.horizontalSymmetry = Rand.Bool;
				CS$<>8__locals1.verticalSymmetry = (!CS$<>8__locals1.horizontalSymmetry || Rand.Bool);
				bool[,] shape = AbstractShapeGenerator.Generate(CS$<>8__locals1.width - 2, CS$<>8__locals1.height - 2, CS$<>8__locals1.horizontalSymmetry, CS$<>8__locals1.verticalSymmetry, true, true, false, 0f);
				Func<int, int, bool> func = (int x, int z) => x >= 0 && z >= 0 && x < shape.GetLength(0) && z < shape.GetLength(1) && shape[x, z];
				for (int k = -1; k < shape.GetLength(0) + 1; k++)
				{
					for (int l = -1; l < shape.GetLength(1) + 1; l++)
					{
						if (!func(k, l) && (func(k - 1, l) || func(k, l - 1) || func(k, l + 1) || func(k + 1, l) || func(k - 1, l - 1) || func(k - 1, l + 1) || func(k + 1, l - 1) || func(k + 1, l + 1)))
						{
							int newX = k + 1;
							int newZ = l + 1;
							CS$<>8__locals1.monument.AddThing(ThingDefOf.Wall, new IntVec3(newX, 0, newZ), Rot4.North, ThingDefOf.WoodLog, 1, null, null, true);
						}
					}
				}
				for (int m = -1; m < shape.GetLength(0) + 1; m++)
				{
					for (int n = -1; n < shape.GetLength(1) + 1; n++)
					{
						if (!func(m, n) && (func(m - 1, n) || func(m, n - 1) || func(m, n + 1) || func(m + 1, n)))
						{
							int num2 = m + 1;
							int num3 = n + 1;
							if ((!func(m - 1, n) && CS$<>8__locals1.monument.Passable(new IntVec3(num2 - 1, 0, num3))) || (!func(m, n - 1) && CS$<>8__locals1.monument.Passable(new IntVec3(num2, 0, num3 - 1))) || (!func(m, n + 1) && CS$<>8__locals1.monument.Passable(new IntVec3(num2, 0, num3 + 1))) || (!func(m + 1, n) && CS$<>8__locals1.monument.Passable(new IntVec3(num2 + 1, 0, num3))))
							{
								list.Add(new IntVec3(num2, 0, num3));
							}
						}
					}
				}
			}
			ResolveParams parms2 = CS$<>8__locals1.parms;
			parms2.sketch = CS$<>8__locals1.monument;
			parms2.connectedGroupsSameStuff = new bool?(true);
			parms2.assignRandomStuffTo = ThingDefOf.Wall;
			SketchResolverDefOf.AssignRandomStuff.Resolve(parms2);
			if (CS$<>8__locals1.parms.addFloors ?? true)
			{
				ResolveParams parms3 = CS$<>8__locals1.parms;
				parms3.singleFloorType = new bool?(true);
				parms3.sketch = CS$<>8__locals1.monument;
				parms3.floorFillRoomsOnly = new bool?(!flag);
				parms3.onlyStoneFloors = new bool?(CS$<>8__locals1.parms.onlyStoneFloors ?? true);
				parms3.allowConcrete = new bool?(CS$<>8__locals1.parms.allowConcrete ?? false);
				parms3.rect = new CellRect?(new CellRect(0, 0, CS$<>8__locals1.width, CS$<>8__locals1.height));
				SketchResolverDefOf.FloorFill.Resolve(parms3);
			}
			if (CS$<>8__locals1.<ResolveInt>g__CanUse|0(ThingDefOf.Column))
			{
				ResolveParams parms4 = CS$<>8__locals1.parms;
				parms4.rect = new CellRect?(new CellRect(0, 0, CS$<>8__locals1.width, CS$<>8__locals1.height));
				parms4.sketch = CS$<>8__locals1.monument;
				parms4.requireFloor = new bool?(true);
				SketchResolverDefOf.AddColumns.Resolve(parms4);
			}
			this.TryPlaceFurniture(CS$<>8__locals1.parms, CS$<>8__locals1.monument, new Func<ThingDef, bool>(CS$<>8__locals1.<ResolveInt>g__CanUse|0));
			for (int num4 = 0; num4 < 2; num4++)
			{
				ResolveParams parms5 = CS$<>8__locals1.parms;
				parms5.addFloors = new bool?(false);
				parms5.sketch = CS$<>8__locals1.monument;
				parms5.rect = new CellRect?(new CellRect(0, 0, CS$<>8__locals1.width, CS$<>8__locals1.height));
				SketchResolverDefOf.AddInnerMonuments.Resolve(parms5);
			}
			bool flag2 = CS$<>8__locals1.parms.allowMonumentDoors ?? (CS$<>8__locals1.filterAllowsAll || CS$<>8__locals1.parms.allowedMonumentThings.Allows(ThingDefOf.Door));
			IntVec3 pos;
			if (flag2 && list.Where(delegate(IntVec3 x)
			{
				if ((!CS$<>8__locals1.horizontalSymmetry || x.x < CS$<>8__locals1.width / 2) && (!CS$<>8__locals1.verticalSymmetry || x.z < CS$<>8__locals1.height / 2))
				{
					if (CS$<>8__locals1.monument.ThingsAt(x).Any((SketchThing y) => y.def == ThingDefOf.Wall))
					{
						return (!CS$<>8__locals1.monument.ThingsAt(new IntVec3(x.x - 1, x.y, x.z)).Any<SketchThing>() && !CS$<>8__locals1.monument.ThingsAt(new IntVec3(x.x + 1, x.y, x.z)).Any<SketchThing>()) || (!CS$<>8__locals1.monument.ThingsAt(new IntVec3(x.x, x.y, x.z - 1)).Any<SketchThing>() && !CS$<>8__locals1.monument.ThingsAt(new IntVec3(x.x, x.y, x.z + 1)).Any<SketchThing>());
					}
				}
				return false;
			}).TryRandomElement(out pos))
			{
				SketchThing sketchThing = CS$<>8__locals1.monument.ThingsAt(pos).FirstOrDefault((SketchThing x) => x.def == ThingDefOf.Wall);
				if (sketchThing != null)
				{
					CS$<>8__locals1.monument.Remove(sketchThing);
					CS$<>8__locals1.monument.AddThing(ThingDefOf.Door, pos, Rot4.North, sketchThing.Stuff, 1, null, null, true);
				}
			}
			this.TryPlaceFurniture(CS$<>8__locals1.parms, CS$<>8__locals1.monument, new Func<ThingDef, bool>(CS$<>8__locals1.<ResolveInt>g__CanUse|0));
			this.ApplySymmetry(CS$<>8__locals1.parms, CS$<>8__locals1.horizontalSymmetry, CS$<>8__locals1.verticalSymmetry, CS$<>8__locals1.monument, CS$<>8__locals1.width, CS$<>8__locals1.height);
			if (flag2 && !flag)
			{
				SketchThing sketchThing2;
				if (!CS$<>8__locals1.monument.Things.Any((SketchThing x) => x.def == ThingDefOf.Door) && (from t in CS$<>8__locals1.monument.Things
				where CS$<>8__locals1.<>4__this.IsWallBorderingEdge(CS$<>8__locals1.monument, t)
				select t).TryRandomElement(out sketchThing2))
				{
					SketchThing sketchThing3 = CS$<>8__locals1.monument.ThingsAt(sketchThing2.pos).FirstOrDefault((SketchThing x) => x.def == ThingDefOf.Wall);
					if (sketchThing3 != null)
					{
						CS$<>8__locals1.monument.Remove(sketchThing3);
					}
					CS$<>8__locals1.monument.AddThing(ThingDefOf.Door, sketchThing2.pos, Rot4.North, sketchThing2.Stuff, 1, null, null, true);
				}
			}
			this.TryAddDoorsToClosedRooms(CS$<>8__locals1.monument);
			List<SketchThing> things = CS$<>8__locals1.monument.Things;
			for (int num5 = 0; num5 < things.Count; num5++)
			{
				if (things[num5].def == ThingDefOf.Wall)
				{
					CS$<>8__locals1.monument.RemoveTerrain(things[num5].pos);
				}
			}
			CS$<>8__locals1.parms.sketch.MergeAt(CS$<>8__locals1.monument, default(IntVec3), Sketch.SpawnPosType.OccupiedCenter, true);
		}

		// Token: 0x0600826C RID: 33388 RVA: 0x002E4220 File Offset: 0x002E2420
		private bool IsWallBorderingEdge(Sketch monument, SketchThing sketchThing)
		{
			return sketchThing.def == ThingDefOf.Wall && ((monument.Passable(sketchThing.pos.x - 1, sketchThing.pos.z) && monument.Passable(sketchThing.pos.x + 1, sketchThing.pos.z) && monument.AnyTerrainAt(sketchThing.pos.x - 1, sketchThing.pos.z) != monument.AnyTerrainAt(sketchThing.pos.x + 1, sketchThing.pos.z)) || (monument.Passable(sketchThing.pos.x, sketchThing.pos.z - 1) && monument.Passable(sketchThing.pos.x, sketchThing.pos.z + 1) && monument.AnyTerrainAt(sketchThing.pos.x, sketchThing.pos.z - 1) != monument.AnyTerrainAt(sketchThing.pos.x, sketchThing.pos.z + 1)));
		}

		// Token: 0x0600826D RID: 33389 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600826E RID: 33390 RVA: 0x002E4348 File Offset: 0x002E2548
		private void ApplySymmetry(ResolveParams parms, bool horizontalSymmetry, bool verticalSymmetry, Sketch monument, int width, int height)
		{
			if (horizontalSymmetry)
			{
				ResolveParams parms2 = parms;
				parms2.sketch = monument;
				parms2.symmetryVertical = new bool?(false);
				parms2.symmetryOrigin = new int?(width / 2);
				parms2.symmetryOriginIncluded = new bool?(width % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms2);
			}
			if (verticalSymmetry)
			{
				ResolveParams parms3 = parms;
				parms3.sketch = monument;
				parms3.symmetryVertical = new bool?(true);
				parms3.symmetryOrigin = new int?(height / 2);
				parms3.symmetryOriginIncluded = new bool?(height % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms3);
			}
		}

		// Token: 0x0600826F RID: 33391 RVA: 0x002E43E8 File Offset: 0x002E25E8
		private void TryPlaceFurniture(ResolveParams parms, Sketch monument, Func<ThingDef, bool> canUseValidator)
		{
			if (canUseValidator == null || canUseValidator(ThingDefOf.Urn))
			{
				ResolveParams parms2 = parms;
				parms2.sketch = monument;
				parms2.cornerThing = ThingDefOf.Urn;
				parms2.requireFloor = new bool?(true);
				SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.SteleLarge))
			{
				ResolveParams parms3 = parms;
				parms3.sketch = monument;
				parms3.thingCentral = ThingDefOf.SteleLarge;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.SteleGrand))
			{
				ResolveParams parms4 = parms;
				parms4.sketch = monument;
				parms4.thingCentral = ThingDefOf.SteleGrand;
				parms4.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms4);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.Table1x2c))
			{
				ResolveParams parms5 = parms;
				parms5.sketch = monument;
				parms5.wallEdgeThing = ThingDefOf.Table1x2c;
				parms5.requireFloor = new bool?(true);
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms5);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.Table2x2c))
			{
				ResolveParams parms6 = parms;
				parms6.sketch = monument;
				parms6.thingCentral = ThingDefOf.Table2x2c;
				parms6.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms6);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.Sarcophagus))
			{
				ResolveParams parms7 = parms;
				parms7.sketch = monument;
				parms7.wallEdgeThing = ThingDefOf.Sarcophagus;
				parms7.requireFloor = new bool?(true);
				parms7.thingCentral = ThingDefOf.Sarcophagus;
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms7);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms7);
			}
		}

		// Token: 0x06008270 RID: 33392 RVA: 0x002E4588 File Offset: 0x002E2788
		private void TryAddDoorsToClosedRooms(Sketch sketch)
		{
			SketchResolver_Monument.<>c__DisplayClass9_0 CS$<>8__locals1 = new SketchResolver_Monument.<>c__DisplayClass9_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.sketch = sketch;
			SketchThing sketchThing = (from t in CS$<>8__locals1.sketch.Things
			where t.def == ThingDefOf.Wall && CS$<>8__locals1.<>4__this.IsWallBorderingEdge(CS$<>8__locals1.sketch, t)
			select t).FirstOrDefault<SketchThing>();
			if (sketchThing == null)
			{
				return;
			}
			SketchResolver_Monument.tmpSeen.Clear();
			CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__FloodFillFrom|1(sketchThing.pos);
			SketchResolver_Monument.tmpCellsToCheck.Clear();
			SketchResolver_Monument.tmpCellsToCheck.AddRange(CS$<>8__locals1.sketch.OccupiedRect.Cells);
			foreach (IntVec3 intVec in SketchResolver_Monument.tmpCellsToCheck)
			{
				if (!SketchResolver_Monument.tmpSeen.Contains(intVec))
				{
					SketchThing sketchThing2 = (from t in CS$<>8__locals1.sketch.ThingsAt(intVec)
					where t.def == ThingDefOf.Wall
					select t).FirstOrDefault<SketchThing>();
					if (sketchThing2 != null && ((CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsConnectedRoomCell|3(intVec + IntVec3.North) && CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsClosedRoomCell|2(intVec + IntVec3.South)) || (CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsConnectedRoomCell|3(intVec + IntVec3.South) && CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsClosedRoomCell|2(intVec + IntVec3.North)) || (CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsConnectedRoomCell|3(intVec + IntVec3.East) && CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsClosedRoomCell|2(intVec + IntVec3.West)) || (CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsConnectedRoomCell|3(intVec + IntVec3.West) && CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__IsClosedRoomCell|2(intVec + IntVec3.East))))
					{
						CS$<>8__locals1.sketch.AddThing(ThingDefOf.Door, sketchThing2.pos, Rot4.North, sketchThing2.Stuff, 1, null, null, true);
						CS$<>8__locals1.<TryAddDoorsToClosedRooms>g__FloodFillFrom|1(sketchThing2.pos);
					}
				}
			}
			SketchResolver_Monument.tmpCellsToCheck.Clear();
			SketchResolver_Monument.tmpSeen.Clear();
			SketchResolver_Monument.extraRoots.Clear();
		}

		// Token: 0x0400512C RID: 20780
		private static readonly SimpleCurve OpenChancePerSizeCurve = new SimpleCurve
		{
			{
				0f,
				1f,
				true
			},
			{
				3f,
				0.85f,
				true
			},
			{
				6f,
				0.2f,
				true
			},
			{
				8f,
				0f,
				true
			}
		};

		// Token: 0x0400512D RID: 20781
		private static HashSet<IntVec3> tmpSeen = new HashSet<IntVec3>();

		// Token: 0x0400512E RID: 20782
		private static List<IntVec3> tmpCellsToCheck = new List<IntVec3>();

		// Token: 0x0400512F RID: 20783
		private static List<IntVec3> extraRoots = new List<IntVec3>();
	}
}
