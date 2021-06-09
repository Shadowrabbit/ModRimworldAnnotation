using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD0 RID: 7632
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x0600A5C8 RID: 42440 RVA: 0x0006DC62 File Offset: 0x0006BE62
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
