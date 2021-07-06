using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E0E RID: 7694
	public class SketchResolver_AddThingsCentral : SketchResolver
	{
		// Token: 0x0600A695 RID: 42645 RVA: 0x0030537C File Offset: 0x0030357C
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect cellRect = parms.rect ?? parms.sketch.OccupiedRect;
			bool allowWood = parms.allowWood ?? true;
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.thingCentral, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.thingCentral));
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
					CellRect cellRect2 = SketchGenUtility.FindBiggestRectAt(c, outerRect, sketch, hashSet, canTraverse);
					if (cellRect2.Width >= parms.thingCentral.size.x + 2 && cellRect2.Height >= parms.thingCentral.size.z + 2 && Rand.Chance(0.4f))
					{
						parms.sketch.AddThing(parms.thingCentral, new IntVec3(cellRect2.CenterCell.x - parms.thingCentral.size.x / 2, 0, cellRect2.CenterCell.z - parms.thingCentral.size.z / 2), Rot4.North, stuff, 1, null, null, false);
					}
				}
			}
			finally
			{
				this.processed.Clear();
			}
		}

		// Token: 0x0600A696 RID: 42646 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x040070F6 RID: 28918
		private HashSet<IntVec3> processed = new HashSet<IntVec3>();

		// Token: 0x040070F7 RID: 28919
		private const float Chance = 0.4f;
	}
}
