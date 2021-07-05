using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200049F RID: 1183
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x060023FB RID: 9211 RVA: 0x000DFE5B File Offset: 0x000DE05B
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000DFE78 File Offset: 0x000DE078
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x040016B2 RID: 5810
		protected int ticksUntilSound;
	}
}
