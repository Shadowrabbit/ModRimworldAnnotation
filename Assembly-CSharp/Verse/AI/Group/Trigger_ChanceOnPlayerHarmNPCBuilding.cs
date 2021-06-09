using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B02 RID: 2818
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x0600421F RID: 16927 RVA: 0x00031469 File Offset: 0x0002F669
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x00188FBC File Offset: 0x001871BC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x04002D63 RID: 11619
		private float chance = 1f;
	}
}
