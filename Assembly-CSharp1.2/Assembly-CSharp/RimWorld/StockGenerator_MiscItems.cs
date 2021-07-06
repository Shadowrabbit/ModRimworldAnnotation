using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001913 RID: 6419
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06008E12 RID: 36370 RVA: 0x0005F180 File Offset: 0x0005D380
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			int count = this.countRange.RandomInRange;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				ThingDef def;
				if (!(from t in DefDatabase<ThingDef>.AllDefs
				where this.HandlesThingDef(t) && t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate
				select t).TryRandomElementByWeight(new Func<ThingDef, float>(this.SelectionWeight), out def))
				{
					yield break;
				}
				yield return this.MakeThing(def);
				num = i;
			}
			yield break;
		}

		// Token: 0x06008E13 RID: 36371 RVA: 0x0005F190 File Offset: 0x0005D390
		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		// Token: 0x06008E14 RID: 36372 RVA: 0x0005F199 File Offset: 0x0005D399
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06008E15 RID: 36373 RVA: 0x0000CE6C File Offset: 0x0000B06C
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
