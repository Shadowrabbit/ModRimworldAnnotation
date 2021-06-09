using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1A RID: 3866
	public class CompProperties_SelfhealHitpoints : CompProperties
	{
		// Token: 0x06005579 RID: 21881 RVA: 0x0003B4DA File Offset: 0x000396DA
		public CompProperties_SelfhealHitpoints()
		{
			this.compClass = typeof(CompSelfhealHitpoints);
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x0003B4F2 File Offset: 0x000396F2
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

		// Token: 0x04003696 RID: 13974
		public int ticksPerHeal;
	}
}
