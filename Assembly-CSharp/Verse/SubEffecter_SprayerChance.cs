using System;

namespace Verse
{
	// Token: 0x02000816 RID: 2070
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06003400 RID: 13312 RVA: 0x00028BFC File Offset: 0x00026DFC
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00151A84 File Offset: 0x0014FC84
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = this.def.chancePerTick;
			if (this.def.spawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
			{
				num *= (float)(B.Thing.def.size.x * B.Thing.def.size.z);
			}
			if (Rand.Value < num)
			{
				base.MakeMote(A, B);
			}
		}
	}
}
