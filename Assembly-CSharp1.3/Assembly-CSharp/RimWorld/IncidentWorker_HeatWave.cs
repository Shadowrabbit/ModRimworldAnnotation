using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C08 RID: 3080
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06004872 RID: 18546 RVA: 0x0017F353 File Offset: 0x0017D553
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_HeatWave.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x0017F370 File Offset: 0x0017D570
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp >= 20f;
		}
	}
}
