using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C72 RID: 3186
	public class ComplexThreatWorker_SleepingMechanoids : ComplexThreatWorker_SleepingThreat
	{
		// Token: 0x06004A57 RID: 19031 RVA: 0x001898C7 File Offset: 0x00187AC7
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfMechanoids);
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x001898EB File Offset: 0x00187AEB
		protected override IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points)
		{
			return PawnUtility.GetCombatPawnKindsForPoints(new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster), points, null);
		}
	}
}
