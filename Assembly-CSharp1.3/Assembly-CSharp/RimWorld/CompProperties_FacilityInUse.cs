using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112C RID: 4396
	public class CompProperties_FacilityInUse : CompProperties
	{
		// Token: 0x060069B1 RID: 27057 RVA: 0x00239FC2 File Offset: 0x002381C2
		public CompProperties_FacilityInUse()
		{
			this.compClass = typeof(CompFacilityInUse);
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x00239FDA File Offset: 0x002381DA
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType == TickerType.Never)
			{
				yield return string.Format("CompProperties_FacilityInUse has parent {0} with tickerType=Never", parentDef);
			}
			if (this.effectInUse != null && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("CompProperties_FacilityInUse has effectInUse but parent {0} has tickerType!=Normal", parentDef);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003B06 RID: 15110
		public float? inUsePowerConsumption;

		// Token: 0x04003B07 RID: 15111
		public EffecterDef effectInUse;
	}
}
