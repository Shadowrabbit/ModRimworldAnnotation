using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121C RID: 4636
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x06006F4D RID: 28493 RVA: 0x00252638 File Offset: 0x00250838
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
					predicate = (<>9__0 = ((ThingDef d) => this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && d.PlayerAcquirable && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(d)) && !generatedDefs.Contains(d)));
				}
				ThingDef chosenThingDef;
				if (!allDefs.Where(predicate).TryRandomElement(out chosenThingDef))
				{
					yield break;
				}
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef), faction))
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

		// Token: 0x06006F4E RID: 28494 RVA: 0x0025264F File Offset: 0x0025084F
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		// Token: 0x04003D84 RID: 15748
		[NoTranslate]
		private string tradeTag;

		// Token: 0x04003D85 RID: 15749
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x04003D86 RID: 15750
		private List<ThingDef> excludedThingDefs;
	}
}
