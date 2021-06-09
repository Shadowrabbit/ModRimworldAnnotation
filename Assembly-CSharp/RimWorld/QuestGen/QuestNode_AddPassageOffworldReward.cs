using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F80 RID: 8064
	public class QuestNode_AddPassageOffworldReward : QuestNode
	{
		// Token: 0x0600ABB8 RID: 43960 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABB9 RID: 43961 RVA: 0x0031FE2C File Offset: 0x0031E02C
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

		// Token: 0x04007504 RID: 29956
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
