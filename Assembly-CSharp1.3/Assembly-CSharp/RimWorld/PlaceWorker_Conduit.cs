using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001540 RID: 5440
	public class PlaceWorker_Conduit : PlaceWorker
	{
		// Token: 0x06008155 RID: 33109 RVA: 0x002DC118 File Offset: 0x002DA318
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			List<Thing> thingList = loc.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.EverTransmitsPower)
				{
					return false;
				}
				if (thingList[i].def.entityDefToBuild != null)
				{
					ThingDef thingDef = thingList[i].def.entityDefToBuild as ThingDef;
					if (thingDef != null && thingDef.EverTransmitsPower)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
