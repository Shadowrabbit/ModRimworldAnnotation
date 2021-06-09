using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001904 RID: 6404
	public class StockGenerator_Techprints : StockGenerator
	{
		// Token: 0x06008DC8 RID: 36296 RVA: 0x0005EF78 File Offset: 0x0005D178
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
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, 1))
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

		// Token: 0x06008DC9 RID: 36297 RVA: 0x0005EF8F File Offset: 0x0005D18F
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains("Techprint");
		}

		// Token: 0x04005A90 RID: 23184
		private List<CountChance> countChances;

		// Token: 0x04005A91 RID: 23185
		private List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
