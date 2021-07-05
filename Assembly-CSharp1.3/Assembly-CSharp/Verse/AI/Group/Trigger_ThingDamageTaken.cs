using System;

namespace Verse.AI.Group
{
	// Token: 0x020006AE RID: 1710
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x06002F69 RID: 12137 RVA: 0x00118DF6 File Offset: 0x00116FF6
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x00118E18 File Offset: 0x00117018
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x04001CF5 RID: 7413
		private Thing thing;

		// Token: 0x04001CF6 RID: 7414
		private float damageFraction = 0.5f;
	}
}
