using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F5 RID: 6133
	public class CompMelter : ThingComp
	{
		// Token: 0x060087BF RID: 34751 RVA: 0x0027C740 File Offset: 0x0027A940
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
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0400570F RID: 22287
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
