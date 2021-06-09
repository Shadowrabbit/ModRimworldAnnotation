using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011FD RID: 4605
	public static class NeutralGroupIncidentUtility
	{
		// Token: 0x060064CA RID: 25802 RVA: 0x001F403C File Offset: 0x001F223C
		public static bool AnyBlockingHostileLord(Map map, Faction forFaction)
		{
			Faction faction = map.ParentFaction ?? Faction.OfPlayer;
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (lords[i].faction != null && lords[i].faction != forFaction && lords[i].faction != faction && lords[i].AnyActivePawn)
				{
					LordJob lordJob = lords[i].LordJob;
					if ((lordJob == null || lordJob.CanBlockHostileVisitors) && !(lordJob is LordJob_VoluntarilyJoinable) && lords[i].faction.HostileTo(forFaction) && !lords[i].faction.HostileTo(faction))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
