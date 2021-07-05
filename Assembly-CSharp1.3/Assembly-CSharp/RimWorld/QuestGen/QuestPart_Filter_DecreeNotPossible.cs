using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200172A RID: 5930
	public class QuestPart_Filter_DecreeNotPossible : QuestPart_Filter
	{
		// Token: 0x060088BE RID: 35006 RVA: 0x00312510 File Offset: 0x00310710
		protected override bool Pass(SignalArgs args)
		{
			Pawn pawn;
			return args.TryGetArg<Pawn>("SUBJECT", out pawn) && (pawn.royalty == null || !pawn.royalty.PossibleDecreeQuests.Contains(this.quest.root));
		}
	}
}
