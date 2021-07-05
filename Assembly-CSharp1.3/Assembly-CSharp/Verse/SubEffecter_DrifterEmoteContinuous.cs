using System;

namespace Verse
{
	// Token: 0x0200005B RID: 91
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x00015A41 File Offset: 0x00013C41
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00015A4B File Offset: 0x00013C4B
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x04000133 RID: 307
		private int ticksUntilMote;
	}
}
