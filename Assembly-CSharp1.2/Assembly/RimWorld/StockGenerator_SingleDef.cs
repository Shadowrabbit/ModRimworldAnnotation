using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F9 RID: 6393
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x06008D93 RID: 36243 RVA: 0x0005ED84 File Offset: 0x0005CF84
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008D94 RID: 36244 RVA: 0x0005ED94 File Offset: 0x0005CF94
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06008D95 RID: 36245 RVA: 0x0005ED9F File Offset: 0x0005CF9F
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.thingDef.tradeability.TraderCanSell())
			{
				yield return this.thingDef + " tradeability doesn't allow traders to sell this thing";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005A65 RID: 23141
		private ThingDef thingDef;
	}
}
