using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE6 RID: 8166
	public class QuestNode_SignalFactionNonPlayer : QuestNode
	{
		// Token: 0x0600AD31 RID: 44337 RVA: 0x00070D18 File Offset: 0x0006EF18
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AD32 RID: 44338 RVA: 0x00326D18 File Offset: 0x00324F18
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_FactionNonPlayer questPart_Filter_FactionNonPlayer = new QuestPart_Filter_FactionNonPlayer();
			questPart_Filter_FactionNonPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter_FactionNonPlayer.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_FactionNonPlayer.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_FactionNonPlayer);
		}

		// Token: 0x040076B4 RID: 30388
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076B5 RID: 30389
		public QuestNode node;

		// Token: 0x040076B6 RID: 30390
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
