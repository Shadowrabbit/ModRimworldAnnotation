using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F6 RID: 6390
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x06008D7C RID: 36220 RVA: 0x0005ECA7 File Offset: 0x0005CEA7
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			ThingDef thingDef = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, base.RandomCountOf(thingDef)))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008D7D RID: 36221 RVA: 0x0005ECB7 File Offset: 0x0005CEB7
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x06008D7E RID: 36222 RVA: 0x0005ECC5 File Offset: 0x0005CEC5
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.thingDefs.Count; i = num + 1)
			{
				if (!this.thingDefs[i].tradeability.TraderCanSell())
				{
					yield return this.thingDefs[i] + " tradeability doesn't allow traders to sell this thing";
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04005A57 RID: 23127
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
