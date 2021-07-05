using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C71 RID: 3185
	public class ComplexThreatWorker_SleepingInsects : ComplexThreatWorker_SleepingThreat
	{
		// Token: 0x06004A54 RID: 19028 RVA: 0x00189873 File Offset: 0x00187A73
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfInsects);
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x00189897 File Offset: 0x00187A97
		protected override IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points)
		{
			return PawnUtility.GetCombatPawnKindsForPoints((PawnKindDef k) => k.RaceProps.Insect, points, null);
		}
	}
}
