using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001225 RID: 4645
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06006F65 RID: 28517 RVA: 0x00252A78 File Offset: 0x00250C78
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
				yield return this.MakeThing(def, faction);
				num = i;
			}
			yield break;
		}

		// Token: 0x06006F66 RID: 28518 RVA: 0x00252A8F File Offset: 0x00250C8F
		protected virtual Thing MakeThing(ThingDef def, Faction faction)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1, faction);
		}

		// Token: 0x06006F67 RID: 28519 RVA: 0x00252A99 File Offset: 0x00250C99
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06006F68 RID: 28520 RVA: 0x0001F15E File Offset: 0x0001D35E
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
