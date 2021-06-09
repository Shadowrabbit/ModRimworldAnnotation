using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200114E RID: 4430
	public interface IIncidentMakerQuestPart
	{
		// Token: 0x0600613B RID: 24891
		IEnumerable<FiringIncident> MakeIntervalIncidents();
	}
}
