using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CD RID: 2509
	public class ThoughtWorker_QuestPart : ThoughtWorker
	{
		// Token: 0x06003E38 RID: 15928 RVA: 0x00154A64 File Offset: 0x00152C64
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			QuestPart_SituationalThought questPart_SituationalThought = this.FindQuestPart(p);
			if (questPart_SituationalThought == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(questPart_SituationalThought.stage);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00154A90 File Offset: 0x00152C90
		public QuestPart_SituationalThought FindQuestPart(Pawn p)
		{
			List<QuestPart_SituationalThought> situationalThoughtQuestParts = Find.QuestManager.SituationalThoughtQuestParts;
			for (int i = 0; i < situationalThoughtQuestParts.Count; i++)
			{
				if (situationalThoughtQuestParts[i].quest.State == QuestState.Ongoing && situationalThoughtQuestParts[i].State == QuestPartState.Enabled && situationalThoughtQuestParts[i].def == this.def && situationalThoughtQuestParts[i].pawn == p)
				{
					return situationalThoughtQuestParts[i];
				}
			}
			return null;
		}
	}
}
