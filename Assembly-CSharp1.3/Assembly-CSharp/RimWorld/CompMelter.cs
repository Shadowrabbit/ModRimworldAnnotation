using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115F RID: 4447
	public class CompMelter : ThingComp
	{
		// Token: 0x06006ADD RID: 27357 RVA: 0x0023E1A8 File Offset: 0x0023C3A8
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature < 0f)
			{
				return;
			}
			int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
			if (num > 0)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x04003B66 RID: 15206
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
