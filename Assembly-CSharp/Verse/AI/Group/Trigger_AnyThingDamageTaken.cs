using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000B11 RID: 2833
	public class Trigger_AnyThingDamageTaken : Trigger
	{
		// Token: 0x0600423D RID: 16957 RVA: 0x000315D6 File Offset: 0x0002F7D6
		public Trigger_AnyThingDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00189394 File Offset: 0x00187594
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				foreach (Thing thing in this.things)
				{
					if (thing.DestroyedOrNull() || (float)thing.HitPoints < (1f - this.damageFraction) * (float)thing.MaxHitPoints)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04002D6C RID: 11628
		private List<Thing> things;

		// Token: 0x04002D6D RID: 11629
		private float damageFraction = 0.5f;
	}
}
