using System;

namespace Verse
{
	// Token: 0x020000A4 RID: 164
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x0000A880 File Offset: 0x00008A80
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0008BA50 File Offset: 0x00089C50
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float chancePerTick = this.def.chancePerTick;
			if (Rand.Value < chancePerTick)
			{
				base.MakeMote(A);
			}
		}
	}
}
