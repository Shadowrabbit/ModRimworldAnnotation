using System;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200158A RID: 5514
	public class SketchResolver_AncientPowerRoom : SketchResolver
	{
		// Token: 0x06008246 RID: 33350 RVA: 0x002E15AF File Offset: 0x002DF7AF
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x06008247 RID: 33351 RVA: 0x002E1D78 File Offset: 0x002DFF78
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient power room"))
			{
				return;
			}
			CellRect value = parms.rect.Value;
			ThingDef ancientGenerator = ThingDefOf.AncientGenerator;
			foreach (IntVec3 intVec in value.Cells)
			{
				if (!Rand.Chance(0.85f))
				{
					CellRect cellRect = GenAdj.OccupiedRect(intVec, ancientGenerator.defaultPlacingRot, ancientGenerator.size);
					bool flag = true;
					foreach (IntVec3 pos in cellRect.ExpandedBy(1))
					{
						if (parms.sketch.ThingsAt(pos).Any<SketchThing>())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						parms.sketch.AddThing(ancientGenerator, intVec, ancientGenerator.defaultPlacingRot, null, 1, null, null, true);
						ScatterDebrisUtility.ScatterAround(intVec, ancientGenerator.size, Rot4.North, parms.sketch, ThingDefOf.Filth_OilSmear, 0.45f, 1, int.MaxValue, null);
					}
				}
			}
			ResolveParams parms2 = parms;
			parms2.thingCentral = ancientGenerator;
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
		}

		// Token: 0x04005116 RID: 20758
		private const float SkipChance = 0.85f;

		// Token: 0x04005117 RID: 20759
		private const float OilSmearChance = 0.45f;
	}
}
