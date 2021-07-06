using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001911 RID: 6417
	public class StockGenerator_BuySlaves : StockGenerator
	{
		// Token: 0x06008E07 RID: 36359 RVA: 0x0005F14D File Offset: 0x0005D34D
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06008E08 RID: 36360 RVA: 0x0005EE61 File Offset: 0x0005D061
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}
	}
}
