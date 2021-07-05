using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001714 RID: 5908
	public class QuestNode_Filter_FactionNonPlayer : QuestNode_Filter
	{
		// Token: 0x06008863 RID: 34915 RVA: 0x0031012D File Offset: 0x0030E32D
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			return new QuestPart_Filter_FactionNonPlayer();
		}
	}
}
