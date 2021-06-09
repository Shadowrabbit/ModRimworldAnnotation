using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B10 RID: 2832
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x0600423B RID: 16955 RVA: 0x000315B5 File Offset: 0x0002F7B5
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00189344 File Offset: 0x00187544
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x04002D6A RID: 11626
		private Thing thing;

		// Token: 0x04002D6B RID: 11627
		private float damageFraction = 0.5f;
	}
}
