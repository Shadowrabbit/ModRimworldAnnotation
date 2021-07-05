using System;

namespace Verse
{
	// Token: 0x020004A2 RID: 1186
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x06002401 RID: 9217 RVA: 0x000E0421 File Offset: 0x000DE621
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilMote = def.initialDelayTicks;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000E0438 File Offset: 0x000DE638
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.moteCount >= this.def.maxMoteCount)
			{
				return;
			}
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
				this.moteCount++;
			}
		}

		// Token: 0x040016B3 RID: 5811
		private int ticksUntilMote;

		// Token: 0x040016B4 RID: 5812
		private int moteCount;
	}
}
