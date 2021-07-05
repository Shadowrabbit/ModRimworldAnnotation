using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001584 RID: 5508
	public class SketchResolver_AddWallEdgeThings : SketchResolver
	{
		// Token: 0x0600822F RID: 33327 RVA: 0x002E0F08 File Offset: 0x002DF108
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect cellRect = parms.rect ?? parms.sketch.OccupiedRect;
			bool allowWood = parms.allowWood ?? true;
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.wallEdgeThing, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.wallEdgeThing));
			Rot4 rot = (parms.wallEdgeThing.size.z > parms.wallEdgeThing.size.x) ? Rot4.North : Rot4.East;
			Rot4 rot2 = (parms.wallEdgeThing.size.z > parms.wallEdgeThing.size.x) ? Rot4.East : Rot4.North;
			CellRect cellRect2 = GenAdj.OccupiedRect(default(IntVec3), rot, parms.wallEdgeThing.size);
			CellRect cellRect3 = GenAdj.OccupiedRect(default(IntVec3), rot2, parms.wallEdgeThing.size);
			bool requireFloor = parms.requireFloor ?? false;
			this.processed.Clear();
			try
			{
				Predicate<IntVec3> <>9__1;
				foreach (IntVec3 c in cellRect.Cells.InRandomOrder(null))
				{
					CellRect outerRect = cellRect;
					Sketch sketch = parms.sketch;
					HashSet<IntVec3> hashSet = this.processed;
					Predicate<IntVec3> canTraverse;
					if ((canTraverse = <>9__1) == null)
					{
						canTraverse = (<>9__1 = ((IntVec3 x) => !parms.sketch.ThingsAt(x).Any<SketchThing>() && (!requireFloor || (parms.sketch.TerrainAt(x) != null && parms.sketch.TerrainAt(x).layerable))));
					}
					CellRect cellRect4 = SketchGenUtility.FindBiggestRectAt(c, outerRect, sketch, hashSet, canTraverse);
					if (cellRect4.Width >= cellRect2.Width && cellRect4.Height >= cellRect2.Height && cellRect4.Width >= cellRect3.Width && cellRect4.Height >= cellRect3.Height && Rand.Chance(parms.chance ?? 0.2f))
					{
						CellRect cellRect5 = new CellRect(cellRect4.minX, cellRect4.CenterCell.z - cellRect2.Height / 2, cellRect2.Width, cellRect2.Height);
						CellRect cellRect6 = new CellRect(cellRect4.maxX - (cellRect2.Width - 1), cellRect4.CenterCell.z - cellRect2.Height / 2, cellRect2.Width, cellRect2.Height);
						CellRect cellRect7 = new CellRect(cellRect4.CenterCell.x - cellRect3.Width / 2, cellRect4.maxZ - (cellRect3.Height - 1), cellRect3.Width, cellRect3.Height);
						CellRect cellRect8 = new CellRect(cellRect4.CenterCell.x - cellRect3.Width / 2, cellRect4.minZ, cellRect3.Width, cellRect3.Height);
						if ((Rand.Bool && this.CanPlaceAt(cellRect5, Rot4.West, parms.sketch)) || this.CanPlaceAt(cellRect6, Rot4.East, parms.sketch))
						{
							if (Rand.Bool && this.CanPlaceAt(cellRect5, Rot4.West, parms.sketch))
							{
								parms.sketch.AddThing(parms.wallEdgeThing, new IntVec3(cellRect5.minX - cellRect2.minX, 0, cellRect5.minZ - cellRect2.minZ), rot, stuff, 1, null, null, false);
							}
							else if (this.CanPlaceAt(cellRect6, Rot4.East, parms.sketch))
							{
								parms.sketch.AddThing(parms.wallEdgeThing, new IntVec3(cellRect6.minX - cellRect2.minX, 0, cellRect6.minZ - cellRect2.minZ), rot.Opposite, stuff, 1, null, null, false);
							}
						}
						else if (Rand.Bool && this.CanPlaceAt(cellRect7, Rot4.North, parms.sketch))
						{
							parms.sketch.AddThing(parms.wallEdgeThing, new IntVec3(cellRect7.minX - cellRect3.minX, 0, cellRect7.minZ - cellRect3.minZ), rot2.Opposite, stuff, 1, null, null, false);
						}
						else if (this.CanPlaceAt(cellRect8, Rot4.South, parms.sketch))
						{
							parms.sketch.AddThing(parms.wallEdgeThing, new IntVec3(cellRect8.minX - cellRect3.minX, 0, cellRect8.minZ - cellRect3.minZ), rot2, stuff, 1, null, null, false);
						}
					}
				}
			}
			finally
			{
				this.processed.Clear();
			}
		}

		// Token: 0x06008230 RID: 33328 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x06008231 RID: 33329 RVA: 0x002E14BC File Offset: 0x002DF6BC
		private bool CanPlaceAt(CellRect rect, Rot4 dir, Sketch sketch)
		{
			foreach (IntVec3 pos in rect.GetEdgeCells(dir))
			{
				if (dir == Rot4.North)
				{
					pos.z++;
				}
				else if (dir == Rot4.South)
				{
					pos.z++;
				}
				else if (dir == Rot4.East)
				{
					pos.x++;
				}
				else
				{
					pos.x--;
				}
				if (!sketch.ThingsAt(pos).Any((SketchThing x) => x.def == ThingDefOf.Wall))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005113 RID: 20755
		private HashSet<IntVec3> processed = new HashSet<IntVec3>();

		// Token: 0x04005114 RID: 20756
		private const float Chance = 0.2f;
	}
}
