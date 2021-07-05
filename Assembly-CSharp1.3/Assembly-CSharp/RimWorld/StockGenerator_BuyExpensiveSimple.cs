using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001221 RID: 4641
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x06006F59 RID: 28505 RVA: 0x002529BC File Offset: 0x00250BBC
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x002529C8 File Offset: 0x00250BC8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && (thingDef == ThingDefOf.InsectJelly || thingDef.BaseMarketValue / thingDef.VolumePerUnit >= this.minValuePerUnit);
		}

		// Token: 0x04003D8B RID: 15755
		public float minValuePerUnit = 15f;
	}
}
