using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F16 RID: 3862
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x0600555E RID: 21854 RVA: 0x0003B379 File Offset: 0x00039579
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x0003B391 File Offset: 0x00039591
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Concat(new object[]
				{
					"CompProximityFuse needs tickerType ",
					TickerType.Rare,
					" or faster, has ",
					parentDef.tickerType
				});
			}
			if (parentDef.CompDefFor<CompExplosive>() == null)
			{
				yield return "CompProximityFuse requires a CompExplosive";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003680 RID: 13952
		public ThingDef target;

		// Token: 0x04003681 RID: 13953
		public float radius;
	}
}
