using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F14 RID: 3860
	public class CompProperties_Power : CompProperties
	{
		// Token: 0x06005552 RID: 21842 RVA: 0x0003B30B File Offset: 0x0003950B
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

		// Token: 0x04003672 RID: 13938
		public bool transmitsPower;

		// Token: 0x04003673 RID: 13939
		public float basePowerConsumption;

		// Token: 0x04003674 RID: 13940
		public bool shortCircuitInRain;

		// Token: 0x04003675 RID: 13941
		public SoundDef soundPowerOn;

		// Token: 0x04003676 RID: 13942
		public SoundDef soundPowerOff;

		// Token: 0x04003677 RID: 13943
		public SoundDef soundAmbientPowered;

		// Token: 0x04003678 RID: 13944
		public SoundDef soundAmbientProducingPower;
	}
}
