using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E21 RID: 7713
	public class SketchResolver_MonumentRuin : SketchResolver
	{
		// Token: 0x0600A6D9 RID: 42713 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600A6DA RID: 42714 RVA: 0x00307C70 File Offset: 0x00305E70
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
