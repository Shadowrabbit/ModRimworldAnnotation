using System;

namespace Verse
{
	// Token: 0x02000815 RID: 2069
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x060033FE RID: 13310 RVA: 0x00028BFC File Offset: 0x00026DFC
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x00151A24 File Offset: 0x0014FC24
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

		// Token: 0x04002405 RID: 9221
		private int ticksUntilMote;

		// Token: 0x04002406 RID: 9222
		private int moteCount;
	}
}
