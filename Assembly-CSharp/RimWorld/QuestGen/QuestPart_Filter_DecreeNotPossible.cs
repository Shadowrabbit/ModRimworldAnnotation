using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002006 RID: 8198
	public class QuestPart_Filter_DecreeNotPossible : QuestPart_Filter
	{
		// Token: 0x0600ADA8 RID: 44456 RVA: 0x00328CB8 File Offset: 0x00326EB8
		protected override bool Pass(SignalArgs args)
		{
			Pawn pawn;
			return args.TryGetArg<Pawn>("SUBJECT", out pawn) && (pawn.royalty == null || !pawn.royalty.PossibleDecreeQuests.Contains(this.quest.root));
		}
	}
}
