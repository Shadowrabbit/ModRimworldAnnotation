using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001009 RID: 4105
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x060060C6 RID: 24774 RVA: 0x0020EA06 File Offset: 0x0020CC06
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x0020EA0D File Offset: 0x0020CC0D
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.TraderCaravanArrival;
			yield return IncidentDefOf.OrbitalTraderArrival;
			yield return IncidentDefOf.WandererJoin;
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}
	}
}
