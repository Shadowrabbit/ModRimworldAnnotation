using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCC RID: 7628
	public class PlaceWorker_Conduit : PlaceWorker
	{
		// Token: 0x0600A5C0 RID: 42432 RVA: 0x00301AC0 File Offset: 0x002FFCC0
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
