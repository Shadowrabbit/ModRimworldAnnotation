using System;

namespace Verse
{
	// Token: 0x02000818 RID: 2072
	public class SubEffecter_SprayerTriggeredDelayed : SubEffecter_SprayerTriggered
	{
		// Token: 0x06003404 RID: 13316 RVA: 0x00028C10 File Offset: 0x00026E10
		public SubEffecter_SprayerTriggeredDelayed(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00028C21 File Offset: 0x00026E21
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.ticksLeft = this.def.initialDelayTicks;
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x00028C34 File Offset: 0x00026E34
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

		// Token: 0x04002407 RID: 9223
		private int ticksLeft = -1;
	}
}
