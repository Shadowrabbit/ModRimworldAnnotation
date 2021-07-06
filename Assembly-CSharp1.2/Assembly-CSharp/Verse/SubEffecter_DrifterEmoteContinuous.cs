using System;

namespace Verse
{
	// Token: 0x020000A3 RID: 163
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0000A880 File Offset: 0x00008A80
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0000A88A File Offset: 0x00008A8A
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x040002A0 RID: 672
		private int ticksUntilMote;
	}
}
