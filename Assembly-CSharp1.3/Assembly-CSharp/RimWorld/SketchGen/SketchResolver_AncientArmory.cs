using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001585 RID: 5509
	public class SketchResolver_AncientArmory : SketchResolver
	{
		// Token: 0x06008233 RID: 33331 RVA: 0x002E15AF File Offset: 0x002DF7AF
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x06008234 RID: 33332 RVA: 0x002E15CC File Offset: 0x002DF7CC
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient armory"))
			{
				return;
			}
			ResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.AncientLockerBank;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
			ResolveParams parms3 = parms;
			parms3.thingCentral = ThingDefOf.AncientSecurityTurret;
			parms3.requireFloor = new bool?(true);
			SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
		}
	}
}
