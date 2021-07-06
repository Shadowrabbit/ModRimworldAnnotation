using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190F RID: 6415
	public class StockGenerator_BuyTradeTag : StockGenerator
	{
		// Token: 0x06008DFC RID: 36348 RVA: 0x0005F0FD File Offset: 0x0005D2FD
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06008DFD RID: 36349 RVA: 0x0005F106 File Offset: 0x0005D306
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tag);
		}

		// Token: 0x04005AAE RID: 23214
		public string tag;
	}
}
