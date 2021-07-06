using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B1 RID: 4529
	public static class DeepDrillInfestationIncidentUtility
	{
		// Token: 0x060063A4 RID: 25508 RVA: 0x001F023C File Offset: 0x001EE43C
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
