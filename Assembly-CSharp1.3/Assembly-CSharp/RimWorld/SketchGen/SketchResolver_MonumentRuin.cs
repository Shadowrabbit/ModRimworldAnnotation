using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001593 RID: 5523
	public class SketchResolver_MonumentRuin : SketchResolver
	{
		// Token: 0x06008273 RID: 33395 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x06008274 RID: 33396 RVA: 0x002E4830 File Offset: 0x002E2A30
		protected override void ResolveInt(ResolveParams parms)
		{
			ResolveParams resolveParams = parms;
			resolveParams.allowWood = new bool?(parms.allowWood ?? false);
			if (resolveParams.allowedMonumentThings == null)
			{
				resolveParams.allowedMonumentThings = new ThingFilter();
				resolveParams.allowedMonumentThings.SetAllowAll(null, true);
			}
			if (ModsConfig.RoyaltyActive)
			{
				resolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
			}
			SketchResolverDefOf.Monument.Resolve(resolveParams);
			SketchResolverDefOf.DamageBuildings.Resolve(parms);
		}
	}
}
