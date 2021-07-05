using System;

namespace Verse
{
	// Token: 0x020004A5 RID: 1189
	public class SubEffecter_SprayerTriggeredDelayed : SubEffecter_SprayerTriggered
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x000E051F File Offset: 0x000DE71F
		public SubEffecter_SprayerTriggeredDelayed(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000E0530 File Offset: 0x000DE730
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.ticksLeft = this.def.initialDelayTicks;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000E0543 File Offset: 0x000DE743
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.ticksLeft == 0)
			{
				base.MakeMote(A, B);
			}
			if (this.ticksLeft >= 0)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x040016B5 RID: 5813
		private int ticksLeft = -1;
	}
}
