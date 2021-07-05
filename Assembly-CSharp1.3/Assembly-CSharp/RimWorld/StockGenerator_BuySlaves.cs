using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001224 RID: 4644
	public class StockGenerator_BuySlaves : StockGenerator
	{
		// Token: 0x06006F62 RID: 28514 RVA: 0x00252A6F File Offset: 0x00250C6F
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06006F63 RID: 28515 RVA: 0x00252615 File Offset: 0x00250815
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}
	}
}
