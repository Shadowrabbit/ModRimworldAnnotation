using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006A0 RID: 1696
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x06002F4C RID: 12108 RVA: 0x001188DD File Offset: 0x00116ADD
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x001188F8 File Offset: 0x00116AF8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x04001CED RID: 7405
		private float chance = 1f;
	}
}
