using System;

namespace Verse
{
	// Token: 0x020004A3 RID: 1187
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06002403 RID: 9219 RVA: 0x000E0497 File Offset: 0x000DE697
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000E04A4 File Offset: 0x000DE6A4
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
