using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001216 RID: 4630
	public class StockGeneratorUtility
	{
		// Token: 0x06006F32 RID: 28466 RVA: 0x00252082 File Offset: 0x00250282
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count, Faction faction)
		{
			if (thingDef.MadeFromStuff)
			{
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					Thing thing = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1, faction);
					if (thing != null)
					{
						yield return thing;
					}
					num = i;
				}
			}
			else
			{
				Thing thing2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count, faction);
				if (thing2 != null)
				{
					yield return thing2;
				}
			}
			yield break;
		}

		// Token: 0x06006F33 RID: 28467 RVA: 0x002520A0 File Offset: 0x002502A0
		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount, Faction faction)
		{
			if (stackCount <= 0)
			{
				return null;
			}
			if (!thingDef.tradeability.TraderCanSell())
			{
				Log.Error("Tried to make non-trader-sellable thing for trader stock: " + thingDef);
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
