using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200158C RID: 5516
	public class SketchResolver_AncientUtilityBuilding : SketchResolver
	{
		// Token: 0x06008250 RID: 33360 RVA: 0x002E1C34 File Offset: 0x002DFE34
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x06008251 RID: 33361 RVA: 0x002E22D4 File Offset: 0x002E04D4
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient utility building"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			IntVec2 intVec = parms.utilityBuildingSize ?? new IntVec2(10, 10);
			ComplexLayout complexLayout = ComplexLayoutGenerator.GenerateRandomLayout(new CellRect(0, 0, intVec.x, intVec.z), 4, 4, 0f, new IntRange?(IntRange.zero), 1);
			ThingDef stuff = BaseGenUtility.RandomCheapWallStuff(Faction.OfAncients, true);
			for (int i = complexLayout.container.minX; i <= complexLayout.container.maxX; i++)
			{
				for (int j = complexLayout.container.minZ; j <= complexLayout.container.maxZ; j++)
				{
					IntVec3 intVec2 = new IntVec3(i, 0, j);
					if (complexLayout.IsWallAt(intVec2))
					{
						sketch.AddThing(ThingDefOf.Wall, intVec2, Rot4.North, stuff, 1, null, null, true);
					}
					else if (complexLayout.IsDoorAt(intVec2))
					{
						sketch.AddThing(ThingDefOf.Door, intVec2, Rot4.North, stuff, 1, null, null, true);
					}
				}
			}
			List<ComplexRoom> rooms = complexLayout.Rooms;
			rooms.SortByDescending((ComplexRoom a) => a.Area);
			ComplexRoom complexRoom = null;
			for (int k = 0; k < rooms.Count; k++)
			{
				if (complexLayout.IsAdjacentToLayoutEdge(rooms[k]))
				{
					foreach (IntVec3 intVec3 in rooms[k].Cells)
					{
						if (complexLayout.container.IsOnEdge(intVec3) && complexLayout.IsWallAt(intVec3))
						{
							sketch.AddThing(ThingDefOf.AncientFence, intVec3, Rot4.North, null, 1, null, null, true);
						}
					}
					IEnumerable<IntVec3> cellsToCheck = rooms[k].rects.SelectMany((CellRect r) => r.Cells).InRandomOrder(null);
					IntVec3 pos;
					if (this.TryFindThingPositionWithGap(ThingDefOf.AncientGenerator, cellsToCheck, sketch, out pos, 1))
					{
						sketch.AddThing(ThingDefOf.AncientGenerator, pos, ThingDefOf.AncientGenerator.defaultPlacingRot, null, 1, null, null, true);
					}
					foreach (CellRect cellRect in rooms[k].rects)
					{
						foreach (IntVec3 pos2 in cellRect)
						{
							sketch.AddTerrain(TerrainDefOf.Concrete, pos2, true);
						}
					}
					complexRoom = rooms[k];
					break;
				}
			}
			for (int l = 0; l < rooms.Count; l++)
			{
				if (rooms[l] != complexRoom)
				{
					foreach (IntVec3 pos3 in rooms[l].rects.SelectMany((CellRect r) => r.Cells))
					{
						sketch.AddTerrain(TerrainDefOf.Concrete, pos3, true);
					}
				}
			}
			parms.sketch.Merge(sketch, true);
			ResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.Table1x2c;
			parms2.requireFloor = new bool?(true);
			parms2.allowWood = new bool?(false);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
			ResolveParams parms3 = parms;
			parms3.destroyChanceExp = new float?(1.5f);
			SketchResolverDefOf.DamageBuildings.Resolve(parms3);
		}

		// Token: 0x06008252 RID: 33362 RVA: 0x002E2718 File Offset: 0x002E0918
		private bool TryFindThingPositionWithGap(ThingDef thingDef, IEnumerable<IntVec3> cellsToCheck, Sketch sketch, out IntVec3 position, int gap = 1)
		{
			foreach (IntVec3 intVec in cellsToCheck)
			{
				CellRect cellRect = GenAdj.OccupiedRect(intVec, thingDef.defaultPlacingRot, thingDef.size).ExpandedBy(gap);
				bool flag = true;
				foreach (IntVec3 pos in cellRect.Cells)
				{
					if (sketch.EdificeAt(pos) != null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					position = intVec;
					return true;
				}
			}
			position = IntVec3.Invalid;
			return false;
		}
	}
}
