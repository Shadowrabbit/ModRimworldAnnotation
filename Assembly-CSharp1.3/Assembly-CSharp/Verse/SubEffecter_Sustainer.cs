using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004A6 RID: 1190
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x0600240A RID: 9226 RVA: 0x000158D1 File Offset: 0x00013AD1
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000E056C File Offset: 0x000DE76C
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

		// Token: 0x040016B6 RID: 5814
		private int age;

		// Token: 0x040016B7 RID: 5815
		private Sustainer sustainer;
	}
}
