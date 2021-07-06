using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FF RID: 6399
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x06008DB9 RID: 36281 RVA: 0x0005EEDC File Offset: 0x0005D0DC
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			Func<ThingDef, bool> <>9__0;
			int num;
			for (int i = 0; i < numThingDefsToUse; i = num + 1)
			{
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef d) => this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(d)) && !generatedDefs.Contains(d)));
				}
				ThingDef chosenThingDef;
				if (!allDefs.Where(predicate).TryRandomElement(out chosenThingDef))
				{
					yield break;
				}
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				generatedDefs.Add(chosenThingDef);
				chosenThingDef = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008DBA RID: 36282 RVA: 0x0005EEEC File Offset: 0x0005D0EC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		// Token: 0x04005A7F RID: 23167
		[NoTranslate]
		private string tradeTag;

		// Token: 0x04005A80 RID: 23168
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x04005A81 RID: 23169
		private List<ThingDef> excludedThingDefs;
	}
}
