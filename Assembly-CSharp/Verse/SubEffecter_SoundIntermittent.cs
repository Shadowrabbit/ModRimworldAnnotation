using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000812 RID: 2066
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x060033F8 RID: 13304 RVA: 0x00028BB6 File Offset: 0x00026DB6
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0015160C File Offset: 0x0014F80C
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x04002404 RID: 9220
		protected int ticksUntilSound;
	}
}
