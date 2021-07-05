using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006AA RID: 1706
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x06002F61 RID: 12129 RVA: 0x00118CCC File Offset: 0x00116ECC
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
