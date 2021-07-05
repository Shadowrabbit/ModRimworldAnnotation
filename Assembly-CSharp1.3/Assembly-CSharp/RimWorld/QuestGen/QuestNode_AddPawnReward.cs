using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016AD RID: 5805
	public class QuestNode_AddPawnReward : QuestNode
	{
		// Token: 0x060086B9 RID: 34489 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086BA RID: 34490 RVA: 0x00304588 File Offset: 0x00302788
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

		// Token: 0x04005482 RID: 21634
		public SlateRef<Pawn> pawn;

		// Token: 0x04005483 RID: 21635
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x04005484 RID: 21636
		public SlateRef<bool> rewardDetailsHidden;
	}
}
