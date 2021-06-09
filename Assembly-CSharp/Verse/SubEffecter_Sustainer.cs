using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000819 RID: 2073
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06003407 RID: 13319 RVA: 0x0000A876 File Offset: 0x00008A76
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x00151AF8 File Offset: 0x0014FCF8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > this.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = this.def.soundDef.TrySpawnSustainer(info);
					return;
				}
				this.sustainer.Maintain();
			}
		}

		// Token: 0x04002408 RID: 9224
		private int age;

		// Token: 0x04002409 RID: 9225
		private Sustainer sustainer;
	}
}
