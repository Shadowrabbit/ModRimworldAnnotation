using System;

namespace RimWorld
{
	// Token: 0x020008D9 RID: 2265
	public class RaidStrategyWorker_ImmediateAttackFriendly : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x06003B7D RID: 15229 RVA: 0x0014C1A1 File Offset: 0x0014A3A1
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
