using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001207 RID: 4615
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x060064E9 RID: 25833 RVA: 0x001F4BDC File Offset: 0x001F2DDC
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

		// Token: 0x060064EA RID: 25834 RVA: 0x000451E6 File Offset: 0x000433E6
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}

		// Token: 0x0400432E RID: 17198
		private const int EnsureMinDurationTicks = 5000;
	}
}
