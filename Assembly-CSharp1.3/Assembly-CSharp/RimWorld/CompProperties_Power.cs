using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A08 RID: 2568
	public class CompProperties_Power : CompProperties
	{
		// Token: 0x06003EF6 RID: 16118 RVA: 0x00157D6C File Offset: 0x00155F6C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.basePowerConsumption > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "PowerConsumption".Translate(), this.basePowerConsumption.ToString("F0") + " W", "Stat_Thing_PowerConsumption_Desc".Translate(), 5000, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x040021EA RID: 8682
		public bool transmitsPower;

		// Token: 0x040021EB RID: 8683
		public float basePowerConsumption;

		// Token: 0x040021EC RID: 8684
		public bool shortCircuitInRain;

		// Token: 0x040021ED RID: 8685
		public bool showPowerNeededIfOff = true;

		// Token: 0x040021EE RID: 8686
		public SoundDef soundPowerOn;

		// Token: 0x040021EF RID: 8687
		public SoundDef soundPowerOff;

		// Token: 0x040021F0 RID: 8688
		public SoundDef soundAmbientPowered;

		// Token: 0x040021F1 RID: 8689
		public SoundDef soundAmbientProducingPower;
	}
}
