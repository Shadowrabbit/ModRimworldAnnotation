using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001589 RID: 5513
	public class SketchResolver_AncientLandingPad : SketchResolver
	{
		// Token: 0x06008243 RID: 33347 RVA: 0x002E1C34 File Offset: 0x002DFE34
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x06008244 RID: 33348 RVA: 0x002E1C40 File Offset: 0x002DFE40
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient landing pad"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			IntVec2 intVec = parms.landingPadSize ?? new IntVec2(12, 12);
			CellRect cellRect = new CellRect(0, 0, intVec.x, intVec.z);
			foreach (IntVec3 pos in cellRect)
			{
				sketch.AddTerrain(TerrainDefOf.Concrete, pos, true);
			}
			foreach (IntVec3 pos2 in cellRect.Corners)
			{
				sketch.AddThing(ThingDefOf.AncientShipBeacon, pos2, Rot4.North, null, 1, null, null, true);
			}
			parms.sketch.Merge(sketch, true);
			ResolveParams parms2 = parms;
			parms2.destroyChanceExp = new float?(5f);
			SketchResolverDefOf.DamageBuildings.Resolve(parms2);
		}
	}
}
