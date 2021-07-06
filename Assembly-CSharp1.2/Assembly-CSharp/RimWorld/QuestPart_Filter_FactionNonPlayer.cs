using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001066 RID: 4198
	public class QuestPart_Filter_FactionNonPlayer : QuestPart_Filter
	{
		// Token: 0x06005B53 RID: 23379 RVA: 0x001D7DDC File Offset: 0x001D5FDC
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
