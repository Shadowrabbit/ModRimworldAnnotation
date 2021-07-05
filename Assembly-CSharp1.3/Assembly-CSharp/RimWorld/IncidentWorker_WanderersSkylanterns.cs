using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C29 RID: 3113
	public class IncidentWorker_WanderersSkylanterns : IncidentWorker_VisitorGroup
	{
		// Token: 0x0600491F RID: 18719 RVA: 0x001836E3 File Offset: 0x001818E3
		protected override LordJob_VisitColony CreateLordJob(IncidentParms parms, List<Pawn> pawns)
		{
			LordJob_VisitColony lordJob_VisitColony = base.CreateLordJob(parms, pawns);
			lordJob_VisitColony.gifts = parms.gifts;
			parms.gifts = null;
			return lordJob_VisitColony;
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x00183700 File Offset: 0x00181900
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, Pawn leader, bool traderExists)
		{
			base.SendStandardLetter(parms, pawns, Array.Empty<NamedArgument>());
		}
	}
}
