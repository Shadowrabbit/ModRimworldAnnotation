using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B0C RID: 2828
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x06004233 RID: 16947 RVA: 0x0018927C File Offset: 0x0018747C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.FactionRelationsChanged)
			{
				FactionRelationKind? previousRelationKind = signal.previousRelationKind;
				FactionRelationKind factionRelationKind = FactionRelationKind.Hostile;
				return (previousRelationKind.GetValueOrDefault() == factionRelationKind & previousRelationKind != null) && lord.faction != null && !lord.faction.HostileTo(Faction.OfPlayer);
			}
			return false;
		}
	}
}
