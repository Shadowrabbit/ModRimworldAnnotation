using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016BF RID: 5823
	public class QuestNode_ExtraFaction : QuestNode
	{
		// Token: 0x060086F0 RID: 34544 RVA: 0x00305250 File Offset: 0x00303450
		protected override void RunInt()
		{
			Faction value = this.faction.GetValue(QuestGen.slate);
			if (value == null)
			{
				Thing value2 = this.factionOf.GetValue(QuestGen.slate);
				if (value2 != null)
				{
					value = value2.Faction;
				}
				if (value == null)
				{
					return;
				}
			}
			QuestGen.quest.AddPart(new QuestPart_ExtraFaction
			{
				affectedPawns = this.pawns.GetValue(QuestGen.slate).ToList<Pawn>(),
				extraFaction = new ExtraFaction(value, this.factionType.GetValue(QuestGen.slate)),
				areHelpers = this.areHelpers.GetValue(QuestGen.slate),
				inSignalRemovePawn = this.inSignalRemovePawn.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x060086F1 RID: 34545 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040054CA RID: 21706
		public SlateRef<Thing> factionOf;

		// Token: 0x040054CB RID: 21707
		public SlateRef<Faction> faction;

		// Token: 0x040054CC RID: 21708
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040054CD RID: 21709
		public SlateRef<ExtraFactionType> factionType;

		// Token: 0x040054CE RID: 21710
		public SlateRef<bool> areHelpers;

		// Token: 0x040054CF RID: 21711
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;
	}
}
