using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001219 RID: 4633
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x06006F40 RID: 28480 RVA: 0x0025255E File Offset: 0x0025075E
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			ThingDef thingDef = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, base.RandomCountOf(thingDef), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006F41 RID: 28481 RVA: 0x00252575 File Offset: 0x00250775
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x06006F42 RID: 28482 RVA: 0x00252583 File Offset: 0x00250783
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

		// Token: 0x04003D80 RID: 15744
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
