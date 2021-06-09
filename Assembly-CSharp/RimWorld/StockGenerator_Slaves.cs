using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FC RID: 6396
	public class StockGenerator_Slaves : StockGenerator
	{
		// Token: 0x06008DAA RID: 36266 RVA: 0x0005EE4A File Offset: 0x0005D04A
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			if (this.respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent)
			{
				yield break;
			}
			int count = this.countRange.RandomInRange;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Faction faction2;
				if (!(from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction && !fac.temporary
				select fac).TryRandomElement(out faction2))
				{
					yield break;
				}
				PawnGenerationRequest request = PawnGenerationRequest.MakeDefault();
				request.KindDef = ((this.slaveKindDef != null) ? this.slaveKindDef : PawnKindDefOf.Slave);
				request.Faction = faction2;
				request.Tile = forTile;
				request.ForceAddFreeWarmLayerIfNeeded = !this.trader.orbital;
				request.RedressValidator = ((Pawn x) => x.royalty == null || !x.royalty.AllTitlesForReading.Any<RoyalTitle>());
				yield return PawnGenerator.GeneratePawn(request);
				num = i;
			}
			yield break;
		}

		// Token: 0x06008DAB RID: 36267 RVA: 0x0005EE61 File Offset: 0x0005D061
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}

		// Token: 0x04005A72 RID: 23154
		private bool respectPopulationIntent;

		// Token: 0x04005A73 RID: 23155
		public PawnKindDef slaveKindDef;
	}
}
