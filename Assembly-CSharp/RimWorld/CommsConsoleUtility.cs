using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016AF RID: 5807
	public static class CommsConsoleUtility
	{
		// Token: 0x06007F2F RID: 32559 RVA: 0x0025C484 File Offset: 0x0025A684
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

		// Token: 0x06007F30 RID: 32560 RVA: 0x0025C4F8 File Offset: 0x0025A6F8
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
