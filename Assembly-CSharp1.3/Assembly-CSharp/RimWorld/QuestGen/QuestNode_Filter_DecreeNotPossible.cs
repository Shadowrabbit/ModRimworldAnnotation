using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001713 RID: 5907
	public class QuestNode_Filter_DecreeNotPossible : QuestNode_Filter
	{
		// Token: 0x06008861 RID: 34913 RVA: 0x00310126 File Offset: 0x0030E326
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			return new QuestPart_Filter_DecreeNotPossible();
		}
	}
}
