using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F81 RID: 8065
	public class QuestNode_AddPawnReward : QuestNode
	{
		// Token: 0x0600ABBB RID: 43963 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABBC RID: 43964 RVA: 0x0031FE8C File Offset: 0x0031E08C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value == null)
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_Pawn
			{
				pawn = value,
				detailsHidden = this.rewardDetailsHidden.GetValue(slate)
			});
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x04007505 RID: 29957
		public SlateRef<Pawn> pawn;

		// Token: 0x04007506 RID: 29958
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x04007507 RID: 29959
		public SlateRef<bool> rewardDetailsHidden;
	}
}
