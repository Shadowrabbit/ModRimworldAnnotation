using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F95 RID: 8085
	public class QuestNode_ExtraFaction : QuestNode
	{
		// Token: 0x0600ABF8 RID: 44024 RVA: 0x00320990 File Offset: 0x0031EB90
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

		// Token: 0x0600ABF9 RID: 44025 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400754A RID: 30026
		public SlateRef<Thing> factionOf;

		// Token: 0x0400754B RID: 30027
		public SlateRef<Faction> faction;

		// Token: 0x0400754C RID: 30028
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400754D RID: 30029
		public SlateRef<ExtraFactionType> factionType;

		// Token: 0x0400754E RID: 30030
		public SlateRef<bool> areHelpers;

		// Token: 0x0400754F RID: 30031
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;
	}
}
