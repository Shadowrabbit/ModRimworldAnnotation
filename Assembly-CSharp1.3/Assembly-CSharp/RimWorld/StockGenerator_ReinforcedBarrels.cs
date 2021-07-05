using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122B RID: 4651
	public class StockGenerator_ReinforcedBarrels : StockGenerator
	{
		// Token: 0x06006F7F RID: 28543 RVA: 0x00252EED File Offset: 0x002510ED
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			if (Find.Storyteller.difficulty.classicMortars)
			{
				yield break;
			}
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(ThingDefOf.ReinforcedBarrel, base.RandomCountOf(ThingDefOf.ReinforcedBarrel), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006F80 RID: 28544 RVA: 0x00252F04 File Offset: 0x00251104
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.ReinforcedBarrel;
		}

		// Token: 0x06006F81 RID: 28545 RVA: 0x00252F0E File Offset: 0x0025110E
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			yield break;
		}
	}
}
