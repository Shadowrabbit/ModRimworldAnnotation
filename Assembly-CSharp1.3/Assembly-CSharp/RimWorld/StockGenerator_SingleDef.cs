using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121A RID: 4634
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x06006F45 RID: 28485 RVA: 0x002525B6 File Offset: 0x002507B6
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006F46 RID: 28486 RVA: 0x002525CD File Offset: 0x002507CD
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06006F47 RID: 28487 RVA: 0x002525D8 File Offset: 0x002507D8
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

		// Token: 0x04003D81 RID: 15745
		private ThingDef thingDef;
	}
}
