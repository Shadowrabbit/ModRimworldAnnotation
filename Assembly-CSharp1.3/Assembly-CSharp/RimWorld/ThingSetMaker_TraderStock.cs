using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EA RID: 4330
	public class ThingSetMaker_TraderStock : ThingSetMaker
	{
		// Token: 0x06006794 RID: 26516 RVA: 0x00230314 File Offset: 0x0022E514
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement<TraderKindDef>();
			Faction makingFaction = parms.makingFaction;
			int forTile;
			if (parms.tile != null)
			{
				forTile = parms.tile.Value;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				forTile = Find.AnyPlayerHomeMap.Tile;
			}
			else if (Find.CurrentMap != null)
			{
				forTile = Find.CurrentMap.Tile;
			}
			else
			{
				forTile = -1;
			}
			for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
			{
				foreach (Thing thing in traderKindDef.stockGenerators[i].GenerateThings(forTile, parms.makingFaction))
				{
					if (!thing.def.tradeability.TraderCanSell())
					{
						Log.Error(string.Concat(new object[]
						{
							traderKindDef,
							" generated carrying ",
							thing,
							" which can't be sold by traders. Ignoring..."
						}));
					}
					else
					{
						thing.PostGeneratedForTrader(traderKindDef, forTile, makingFaction);
						outThings.Add(thing);
					}
				}
			}
		}

		// Token: 0x06006795 RID: 26517 RVA: 0x00230444 File Offset: 0x0022E644
		public float DebugAverageTotalStockValue(TraderKindDef td)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = td;
			parms.tile = new int?(-1);
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				foreach (Thing thing in base.Generate(parms))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			return num / 50f;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x002304DC File Offset: 0x0022E6DC
		public string DebugGenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.DebugAverageTotalStockValue(td).ToString("F0"));
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = td;
			parms.tile = new int?(-1);
			(from x in Find.FactionManager.AllFactionsListForReading
			where x.def.baseTraderKinds.Contains(td) || x.def.visitorTraderKinds.Contains(td) || x.def.caravanTraderKinds.Contains(td)
			select x).TryRandomElement(out parms.makingFaction);
			stringBuilder.AppendLine("Example generated stock:\n\n");
			foreach (Thing thing in base.Generate(parms))
			{
				MinifiedThing minifiedThing = thing as MinifiedThing;
				Thing thing2;
				if (minifiedThing != null)
				{
					thing2 = minifiedThing.InnerThing;
				}
				else
				{
					thing2 = thing;
				}
				string text = thing2.LabelCap;
				text = text + " [" + (thing2.MarketValue * (float)thing2.stackCount).ToString("F0") + "]";
				stringBuilder.AppendLine(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x00230634 File Offset: 0x0022E834
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			if (parms.traderDef == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < parms.traderDef.stockGenerators.Count; i = num + 1)
			{
				StockGenerator stock = parms.traderDef.stockGenerators[i];
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				Func<ThingDef, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef x) => x.tradeability.TraderCanSell() && stock.HandlesThingDef(x)));
				}
				foreach (ThingDef thingDef in allDefs.Where(predicate))
				{
					yield return thingDef;
				}
				IEnumerator<ThingDef> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}
	}
}
