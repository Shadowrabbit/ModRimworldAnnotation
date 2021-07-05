using System;

namespace Verse
{
	// Token: 0x0200005C RID: 92
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x00015A41 File Offset: 0x00013C41
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00015A7C File Offset: 0x00013C7C
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
