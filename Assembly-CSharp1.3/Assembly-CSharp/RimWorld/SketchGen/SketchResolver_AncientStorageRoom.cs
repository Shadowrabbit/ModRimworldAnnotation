using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200158B RID: 5515
	public class SketchResolver_AncientStorageRoom : SketchResolver
	{
		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x06008249 RID: 33353 RVA: 0x002E1ED8 File Offset: 0x002E00D8
		private static IEnumerable<ThingDef> PossibleBuildings
		{
			get
			{
				yield return ThingDefOf.AncientCrate;
				yield return ThingDefOf.AncientBarrel;
				yield break;
			}
		}

		// Token: 0x0600824A RID: 33354 RVA: 0x002E15AF File Offset: 0x002DF7AF
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600824B RID: 33355 RVA: 0x002E1EE4 File Offset: 0x002E00E4
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient storage room"))
			{
				return;
			}
			CellRect rect = parms.rect.Value;
			CellRect lhs = SketchGenUtility.FindBiggestRect(parms.sketch, (IntVec3 p) => rect.Contains(p) && !parms.sketch.ThingsAt(p).Any<SketchThing>(), null, 3);
			if (lhs == CellRect.Empty)
			{
				return;
			}
			SketchResolver_AncientStorageRoom.tmpCells.Clear();
			Rot4 dir = (lhs.Width > lhs.Height) ? Rot4.North : Rot4.East;
			SketchResolver_AncientStorageRoom.tmpCells.AddRange(lhs.GetEdgeCells(dir));
			SketchResolver_AncientStorageRoom.tmpCells.AddRange(lhs.GetEdgeCells(dir.Opposite));
			ThingDef def = SketchResolver_AncientStorageRoom.PossibleBuildings.RandomElement<ThingDef>();
			foreach (IntVec3 intVec in SketchResolver_AncientStorageRoom.tmpCells)
			{
				if (Rand.Chance(SketchResolver_AncientStorageRoom.WallChance) && this.CanPlaceWallAdjacentAt(intVec, parms.sketch))
				{
					parms.sketch.AddThing(def, intVec, Rot4.North, null, 1, null, null, true);
				}
			}
			SketchResolver_AncientStorageRoom.tmpCells.Clear();
			foreach (IntVec3 center in rect.Cells)
			{
				CellRect rect2 = CellRect.CenteredOn(center, 2, 2);
				if (this.CanPlaceAt(rect2, parms.sketch))
				{
					foreach (IntVec3 intVec2 in rect2)
					{
						ThingDef thingDef = SketchResolver_AncientStorageRoom.PossibleBuildings.RandomElement<ThingDef>();
						parms.sketch.AddThing(thingDef, intVec2, Rot4.North, null, 1, null, null, true);
						ScatterDebrisUtility.ScatterAround(intVec2, thingDef.size, Rot4.North, parms.sketch, ThingDefOf.Filth_OilSmear, 0.15f, 1, int.MaxValue, null);
					}
				}
			}
		}

		// Token: 0x0600824C RID: 33356 RVA: 0x002E2150 File Offset: 0x002E0350
		private bool CanPlaceAt(CellRect rect, Sketch sketch)
		{
			foreach (IntVec3 pos in rect.Cells)
			{
				if (sketch.ThingsAt(pos).Any<SketchThing>())
				{
					return false;
				}
			}
			foreach (IntVec3 pos2 in rect.ExpandedBy(1).EdgeCells)
			{
				if (sketch.ThingsAt(pos2).Any((SketchThing t) => t.def != ThingDefOf.Wall && t.def != ThingDefOf.Door))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600824D RID: 33357 RVA: 0x002E2220 File Offset: 0x002E0420
		private bool CanPlaceWallAdjacentAt(IntVec3 position, Sketch sketch)
		{
			bool result = false;
			IntVec3[] cardinalDirectionsAndInside = GenAdj.CardinalDirectionsAndInside;
			for (int i = 0; i < cardinalDirectionsAndInside.Length; i++)
			{
				IntVec3 pos = cardinalDirectionsAndInside[i] + position;
				foreach (SketchThing sketchThing in sketch.ThingsAt(pos))
				{
					if (sketchThing.def == ThingDefOf.Wall)
					{
						result = true;
					}
					else if (sketchThing.def == ThingDefOf.Door)
					{
						return false;
					}
				}
			}
			return result;
		}

		// Token: 0x04005118 RID: 20760
		private static float WallChance = 0.8f;

		// Token: 0x04005119 RID: 20761
		private const float OilSmearChance = 0.15f;

		// Token: 0x0400511A RID: 20762
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
