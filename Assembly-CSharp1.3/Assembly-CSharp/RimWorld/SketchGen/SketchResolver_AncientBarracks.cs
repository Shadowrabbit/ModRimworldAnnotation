using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001586 RID: 5510
	public class SketchResolver_AncientBarracks : SketchResolver
	{
		// Token: 0x06008236 RID: 33334 RVA: 0x002E15AF File Offset: 0x002DF7AF
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x06008237 RID: 33335 RVA: 0x002E1634 File Offset: 0x002DF834
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient barracks"))
			{
				return;
			}
			CellRect rect = parms.rect.Value;
			ResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			ThingDef ancientBed = ThingDefOf.AncientBed;
			CellRect lhs = SketchGenUtility.FindBiggestRect(parms.sketch, (IntVec3 p) => rect.Contains(p) && !parms.sketch.ThingsAt(p).Any<SketchThing>(), null, 3);
			if (lhs == CellRect.Empty)
			{
				return;
			}
			ResolveParams parms3 = parms;
			parms3.chance = new float?(1f);
			parms3.wallEdgeThing = ThingDefOf.AncientLockerBank;
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms3);
			SketchResolver_AncientBarracks.tmpCells.Clear();
			if (lhs.Width > lhs.Height)
			{
				SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.North));
				SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.South));
				using (List<IntVec3>.Enumerator enumerator = SketchResolver_AncientBarracks.tmpCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 intVec = enumerator.Current;
						if (this.CanPlaceBedAt(ancientBed, intVec, Rot4.North, parms.sketch))
						{
							parms.sketch.AddThing(ancientBed, intVec, Rot4.North, null, 1, null, null, false);
						}
						if (this.CanPlaceBedAt(ancientBed, intVec, Rot4.South, parms.sketch))
						{
							parms.sketch.AddThing(ancientBed, intVec, Rot4.South, null, 1, null, null, false);
						}
					}
					goto IL_2CF;
				}
			}
			SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.East));
			SketchResolver_AncientBarracks.tmpCells.AddRange(lhs.GetEdgeCells(Rot4.West));
			foreach (IntVec3 intVec2 in SketchResolver_AncientBarracks.tmpCells)
			{
				if (this.CanPlaceBedAt(ancientBed, intVec2, Rot4.East, parms.sketch))
				{
					parms.sketch.AddThing(ancientBed, intVec2, Rot4.East, null, 1, null, null, false);
				}
				if (this.CanPlaceBedAt(ancientBed, intVec2, Rot4.West, parms.sketch))
				{
					parms.sketch.AddThing(ancientBed, intVec2, Rot4.West, null, 1, null, null, false);
				}
			}
			IL_2CF:
			SketchResolver_AncientBarracks.tmpCells.Clear();
		}

		// Token: 0x06008238 RID: 33336 RVA: 0x002E1938 File Offset: 0x002DFB38
		private bool CanPlaceBedAt(ThingDef def, IntVec3 position, Rot4 rot, Sketch sketch)
		{
			CellRect cellRect = GenAdj.OccupiedRect(position, rot, def.size);
			foreach (IntVec3 pos in cellRect.Cells)
			{
				if (sketch.ThingsAt(pos).Any((SketchThing x) => x.def == ThingDefOf.Wall))
				{
					return false;
				}
			}
			bool result = false;
			foreach (IntVec3 pos2 in cellRect.ExpandedBy(1).EdgeCells)
			{
				using (IEnumerator<SketchThing> enumerator2 = sketch.ThingsAt(pos2).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.def != ThingDefOf.Wall)
						{
							return false;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x04005115 RID: 20757
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
