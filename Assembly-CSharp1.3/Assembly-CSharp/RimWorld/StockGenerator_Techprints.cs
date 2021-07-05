using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121F RID: 4639
	public class StockGenerator_Techprints : StockGenerator
	{
		// Token: 0x06006F51 RID: 28497 RVA: 0x00252705 File Offset: 0x00250905
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			this.tmpGenerated.Clear();
			int countToGenerate = CountChanceUtility.RandomCount(this.countChances);
			int num;
			for (int i = 0; i < countToGenerate; i = num + 1)
			{
				ThingDef thingDef;
				if (!TechprintUtility.TryGetTechprintDefToGenerate(faction, out thingDef, this.tmpGenerated, 3.4028235E+38f))
				{
					yield break;
				}
				this.tmpGenerated.Add(thingDef);
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, 1, faction))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				num = i;
			}
			this.tmpGenerated.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06006F52 RID: 28498 RVA: 0x0025271C File Offset: 0x0025091C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains("Techprint");
		}

		// Token: 0x04003D89 RID: 15753
		private List<CountChance> countChances;

		// Token: 0x04003D8A RID: 15754
		private List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
