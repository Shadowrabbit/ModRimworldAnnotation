using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001223 RID: 4643
	public class StockGenerator_BuyTradeTag : StockGenerator
	{
		// Token: 0x06006F5F RID: 28511 RVA: 0x00252A49 File Offset: 0x00250C49
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06006F60 RID: 28512 RVA: 0x00252A52 File Offset: 0x00250C52
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tag);
		}

		// Token: 0x04003D8D RID: 15757
		public string tag;
	}
}
