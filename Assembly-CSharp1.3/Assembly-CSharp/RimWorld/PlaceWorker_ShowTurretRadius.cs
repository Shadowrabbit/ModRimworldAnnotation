using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001545 RID: 5445
	public class PlaceWorker_ShowTurretRadius : PlaceWorker
	{
		// Token: 0x0600815F RID: 33119 RVA: 0x002DC250 File Offset: 0x002DA450
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			VerbProperties verbProperties = ((ThingDef)checkingDef).building.turretGunDef.Verbs.Find((VerbProperties v) => v.verbClass == typeof(Verb_Shoot));
			if (verbProperties.range > 0f)
			{
				GenDraw.DrawRadiusRing(loc, verbProperties.range);
			}
			if (verbProperties.minRange > 0f)
			{
				GenDraw.DrawRadiusRing(loc, verbProperties.minRange);
			}
			return true;
		}
	}
}
