using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006AB RID: 1707
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x06002F63 RID: 12131 RVA: 0x00118D1E File Offset: 0x00116F1E
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
