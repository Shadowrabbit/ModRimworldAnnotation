using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190B RID: 6411
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x06008DE6 RID: 36326 RVA: 0x0005F079 File Offset: 0x0005D279
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06008DE7 RID: 36327 RVA: 0x00290644 File Offset: 0x0028E844
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && (thingDef == ThingDefOf.InsectJelly || thingDef.BaseMarketValue / thingDef.VolumePerUnit >= this.minValuePerUnit);
		}

		// Token: 0x04005AA6 RID: 23206
		public float minValuePerUnit = 15f;
	}
}
