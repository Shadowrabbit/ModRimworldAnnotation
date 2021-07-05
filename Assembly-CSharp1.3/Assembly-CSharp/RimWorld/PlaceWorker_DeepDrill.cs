using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001544 RID: 5444
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x0600815D RID: 33117 RVA: 0x002DC223 File Offset: 0x002DA423
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (DeepDrillUtility.GetNextResource(loc, map) == null)
			{
				return "MustPlaceOnDrillable".Translate();
			}
			return true;
		}
	}
}
