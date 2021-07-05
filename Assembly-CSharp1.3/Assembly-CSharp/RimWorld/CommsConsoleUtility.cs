using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001073 RID: 4211
	public static class CommsConsoleUtility
	{
		// Token: 0x060063F3 RID: 25587 RVA: 0x0021BDD0 File Offset: 0x00219FD0
		public static bool PlayerHasPoweredCommsConsole(Map map)
		{
			foreach (Building_CommsConsole building_CommsConsole in map.listerBuildings.AllBuildingsColonistOfClass<Building_CommsConsole>())
			{
				if (building_CommsConsole.Faction == Faction.OfPlayer)
				{
					CompPowerTrader compPowerTrader = building_CommsConsole.TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060063F4 RID: 25588 RVA: 0x0021BE44 File Offset: 0x0021A044
		public static bool PlayerHasPoweredCommsConsole()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
