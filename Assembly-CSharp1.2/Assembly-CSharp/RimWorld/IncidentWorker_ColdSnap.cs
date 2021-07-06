using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BF RID: 4543
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x060063D4 RID: 25556 RVA: 0x000448B8 File Offset: 0x00042AB8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_ColdSnap.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x000448D5 File Offset: 0x00042AD5
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f;
		}
	}
}
