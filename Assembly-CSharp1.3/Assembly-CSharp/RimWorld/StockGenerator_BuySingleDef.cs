using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001222 RID: 4642
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x06006F5C RID: 28508 RVA: 0x00252A35 File Offset: 0x00250C35
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x00252A3E File Offset: 0x00250C3E
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x04003D8C RID: 15756
		public ThingDef thingDef;
	}
}
