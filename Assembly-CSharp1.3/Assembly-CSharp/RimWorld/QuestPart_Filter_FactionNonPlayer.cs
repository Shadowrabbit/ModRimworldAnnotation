using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B39 RID: 2873
	public class QuestPart_Filter_FactionNonPlayer : QuestPart_Filter
	{
		// Token: 0x06004345 RID: 17221 RVA: 0x00167178 File Offset: 0x00165378
		protected override bool Pass(SignalArgs args)
		{
			Faction faction;
			if (args.TryGetArg<Faction>("FACTION", out faction))
			{
				return faction != Faction.OfPlayer;
			}
			Thing thing;
			return args.TryGetArg<Thing>("SUBJECT", out thing) && thing.Faction != Faction.OfPlayer;
		}
	}
}
