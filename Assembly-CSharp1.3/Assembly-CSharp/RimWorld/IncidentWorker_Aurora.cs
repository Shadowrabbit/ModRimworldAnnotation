using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2A RID: 3114
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06004922 RID: 18722 RVA: 0x0018371C File Offset: 0x0018191C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && !this.AuroraWillEndSoon(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x0018376B File Offset: 0x0018196B
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}

		// Token: 0x04002C7D RID: 11389
		private const int EnsureMinDurationTicks = 5000;
	}
}
