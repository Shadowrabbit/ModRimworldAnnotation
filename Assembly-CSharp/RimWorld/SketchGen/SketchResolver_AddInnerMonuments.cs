using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E0B RID: 7691
	public class SketchResolver_AddInnerMonuments : SketchResolver
	{
		// Token: 0x0600A68F RID: 42639 RVA: 0x003051E8 File Offset: 0x003033E8
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect outerRect = parms.rect ?? parms.sketch.OccupiedRect;
			HashSet<IntVec3> processed = new HashSet<IntVec3>();
			using (IEnumerator<IntVec3> enumerator = outerRect.Cells.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					CellRect cellRect = SketchGenUtility.FindBiggestRectAt(c, outerRect, parms.sketch, processed, (IntVec3 x) => !parms.sketch.ThingsAt(x).Any<SketchThing>() && parms.sketch.AnyTerrainAt(c));
					if (cellRect.Width >= 7 && cellRect.Height >= 7)
					{
						int newX = Rand.RangeInclusive(5, cellRect.Width - 2);
						int newZ = Rand.RangeInclusive(5, cellRect.Height - 2);
						Sketch sketch = new Sketch();
						ResolveParams parms2 = parms;
						parms2.sketch = sketch;
						parms2.monumentSize = new IntVec2?(new IntVec2(newX, newZ));
						parms2.rect = null;
						SketchResolverDefOf.Monument.Resolve(parms2);
						parms.sketch.MergeAt(sketch, cellRect.CenterCell, Sketch.SpawnPosType.OccupiedCenter, false);
					}
				}
			}
		}

		// Token: 0x0600A690 RID: 42640 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x040070F1 RID: 28913
		private const int MinRectWidth = 7;

		// Token: 0x040070F2 RID: 28914
		private const int MinRectHeight = 7;
	}
}
