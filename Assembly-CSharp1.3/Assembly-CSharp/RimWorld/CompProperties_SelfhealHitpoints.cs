using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A0B RID: 2571
	public class CompProperties_SelfhealHitpoints : CompProperties
	{
		// Token: 0x06003F02 RID: 16130 RVA: 0x00157E70 File Offset: 0x00156070
		public CompProperties_SelfhealHitpoints()
		{
			this.compClass = typeof(CompSelfhealHitpoints);
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00157E88 File Offset: 0x00156088
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType == TickerType.Rare && this.ticksPerHeal % 250 != 0)
			{
				yield return "TickerType is set to Rare, but ticksPerHeal value is not multiple of " + 250;
			}
			if (parentDef.tickerType == TickerType.Long && this.ticksPerHeal % 2000 != 0)
			{
				yield return "TickerType is set to Long, but ticksPerHeal value is not multiple of " + 2000;
			}
			if (parentDef.tickerType == TickerType.Never)
			{
				yield return "has CompSelfhealHitpoints, but its TickerType is set to Never";
			}
			yield break;
			yield break;
		}

		// Token: 0x040021FA RID: 8698
		public int ticksPerHeal;
	}
}
