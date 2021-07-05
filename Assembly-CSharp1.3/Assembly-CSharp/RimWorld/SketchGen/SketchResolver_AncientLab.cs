using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001588 RID: 5512
	public class SketchResolver_AncientLab : SketchResolver
	{
		// Token: 0x06008240 RID: 33344 RVA: 0x002E15AF File Offset: 0x002DF7AF
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x06008241 RID: 33345 RVA: 0x002E1B70 File Offset: 0x002DFD70
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient lab"))
			{
				return;
			}
			ResolveParams parms2 = parms;
			parms2.thingCentral = ThingDefOf.AncientOperatingTable;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
			ResolveParams parms3 = parms;
			parms3.wallEdgeThing = ThingDefOf.AncientDisplayBank;
			parms3.requireFloor = new bool?(true);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms3);
			foreach (IntVec3 pos in parms.rect.Value.Cells)
			{
				parms.sketch.AddTerrain(TerrainDefOf.MetalTile, pos, true);
			}
		}
	}
}
