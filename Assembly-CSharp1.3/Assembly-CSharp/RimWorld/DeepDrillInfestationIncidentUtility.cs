using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C01 RID: 3073
	public static class DeepDrillInfestationIncidentUtility
	{
		// Token: 0x06004850 RID: 18512 RVA: 0x0017E62C File Offset: 0x0017C82C
		public static void GetUsableDeepDrills(Map map, List<Thing> outDrills)
		{
			outDrills.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
			Faction ofPlayer = Faction.OfPlayer;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == ofPlayer && list[i].TryGetComp<CompCreatesInfestations>().CanCreateInfestationNow)
				{
					outDrills.Add(list[i]);
				}
			}
		}
	}
}
