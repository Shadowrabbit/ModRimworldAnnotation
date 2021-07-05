using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000BC7 RID: 3015
	public interface IIncidentMakerQuestPart
	{
		// Token: 0x0600469B RID: 18075
		IEnumerable<FiringIncident> MakeIntervalIncidents();
	}
}
