using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C09 RID: 3081
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06004875 RID: 18549 RVA: 0x0017F38F File Offset: 0x0017D58F
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_ColdSnap.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x0017F3AC File Offset: 0x0017D5AC
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f;
		}
	}
}
