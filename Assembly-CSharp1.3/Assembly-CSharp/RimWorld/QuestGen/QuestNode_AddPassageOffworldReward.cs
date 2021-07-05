using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016AC RID: 5804
	public class QuestNode_AddPassageOffworldReward : QuestNode
	{
		// Token: 0x060086B6 RID: 34486 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086B7 RID: 34487 RVA: 0x00304528 File Offset: 0x00302728
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_PassageOffworld());
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x04005481 RID: 21633
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
