using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190D RID: 6413
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x06008DF1 RID: 36337 RVA: 0x0005F0BF File Offset: 0x0005D2BF
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06008DF2 RID: 36338 RVA: 0x0005F0C8 File Offset: 0x0005D2C8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x04005AAA RID: 23210
		public ThingDef thingDef;
	}
}
