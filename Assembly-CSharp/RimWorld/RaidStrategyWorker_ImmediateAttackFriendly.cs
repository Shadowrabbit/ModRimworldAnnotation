using System;

namespace RimWorld
{
	// Token: 0x02000E23 RID: 3619
	public class RaidStrategyWorker_ImmediateAttackFriendly : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x0600521A RID: 21018 RVA: 0x00039690 File Offset: 0x00037890
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
