using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E8 RID: 6376
	public class StockGeneratorUtility
	{
		// Token: 0x06008D42 RID: 36162 RVA: 0x0005EA82 File Offset: 0x0005CC82
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count)
		{
			if (thingDef.MadeFromStuff)
			{
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					Thing thing = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1);
					if (thing != null)
					{
						yield return thing;
					}
					num = i;
				}
			}
			else
			{
				Thing thing2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count);
				if (thing2 != null)
				{
					yield return thing2;
				}
			}
			yield break;
		}

		// Token: 0x06008D43 RID: 36163 RVA: 0x0028EC34 File Offset: 0x0028CE34
		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount)
		{
			if (stackCount <= 0)
			{
				return null;
			}
			if (!thingDef.tradeability.TraderCanSell())
			{
				Log.Error("Tried to make non-trader-sellable thing for trader stock: " + thingDef, false);
				return null;
			}
			ThingDef stuff = null;
			if (thingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined)
				where !PawnWeaponGenerator.IsDerpWeapon(thingDef, x)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, stuff);
			thing.stackCount = stackCount;
			return thing;
		}
	}
}
