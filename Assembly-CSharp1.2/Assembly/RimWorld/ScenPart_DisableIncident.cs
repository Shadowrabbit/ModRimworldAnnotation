using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020015FC RID: 5628
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06007A68 RID: 31336 RVA: 0x0005248B File Offset: 0x0005068B
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x06007A69 RID: 31337 RVA: 0x00052492 File Offset: 0x00050692
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
