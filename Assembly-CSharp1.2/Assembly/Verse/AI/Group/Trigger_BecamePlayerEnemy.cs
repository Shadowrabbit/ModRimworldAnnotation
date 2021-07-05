using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B0D RID: 2829
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x06004235 RID: 16949 RVA: 0x00031552 File Offset: 0x0002F752
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
