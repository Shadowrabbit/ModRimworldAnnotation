using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C25 RID: 3109
	public static class NeutralGroupIncidentUtility
	{
		// Token: 0x0600490A RID: 18698 RVA: 0x00182A5C File Offset: 0x00180C5C
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
