using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A09 RID: 2569
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x06003EF9 RID: 16121 RVA: 0x00157D9B File Offset: 0x00155F9B
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00157DB3 File Offset: 0x00155FB3
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

		// Token: 0x040021F2 RID: 8690
		public ThingDef target;

		// Token: 0x040021F3 RID: 8691
		public float radius;
	}
}
