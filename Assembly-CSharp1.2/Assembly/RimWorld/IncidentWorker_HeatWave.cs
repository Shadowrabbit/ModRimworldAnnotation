using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BE RID: 4542
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x060063D1 RID: 25553 RVA: 0x0004487C File Offset: 0x00042A7C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_HeatWave.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x00044899 File Offset: 0x00042A99
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp >= 20f;
		}
	}
}
