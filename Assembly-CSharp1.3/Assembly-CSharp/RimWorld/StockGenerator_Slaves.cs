using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121B RID: 4635
	public class StockGenerator_Slaves : StockGenerator
	{
		// Token: 0x06006F4A RID: 28490 RVA: 0x002525F7 File Offset: 0x002507F7
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			if (this.respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent)
			{
				yield break;
			}
			if (faction != null && faction.ideos != null)
			{
				bool flag = true;
				using (IEnumerator<Ideo> enumerator = faction.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IdeoApprovesOfSlavery())
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					yield break;
				}
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
				yield return PawnGenerator.GeneratePawn(request);
				num = i;
			}
			yield break;
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x00252615 File Offset: 0x00250815
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}

		// Token: 0x04003D82 RID: 15746
		private bool respectPopulationIntent;

		// Token: 0x04003D83 RID: 15747
		public PawnKindDef slaveKindDef;
	}
}
