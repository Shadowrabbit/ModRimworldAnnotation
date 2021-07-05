using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020006AF RID: 1711
	public class Trigger_AnyThingDamageTaken : Trigger
	{
		// Token: 0x06002F6B RID: 12139 RVA: 0x00118E66 File Offset: 0x00117066
		public Trigger_AnyThingDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x00118E88 File Offset: 0x00117088
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

		// Token: 0x04001CF7 RID: 7415
		private List<Thing> things;

		// Token: 0x04001CF8 RID: 7416
		private float damageFraction = 0.5f;
	}
}
