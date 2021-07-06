using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F9B RID: 8091
	public class QuestNode_GiveRoyalFavor : QuestNode
	{
		// Token: 0x0600AC10 RID: 44048 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC11 RID: 44049 RVA: 0x00320D00 File Offset: 0x0031EF00
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.giveTo = this.giveTo.GetValue(slate);
			questPart_GiveRoyalFavor.giveToAccepter = this.giveToAccepter.GetValue(slate);
			questPart_GiveRoyalFavor.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_GiveRoyalFavor.amount = this.amount.GetValue(slate);
			questPart_GiveRoyalFavor.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GiveRoyalFavor);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = questPart_GiveRoyalFavor.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(questPart_GiveRoyalFavor);
				choice.rewards.Add(new Reward_RoyalFavor
				{
					faction = questPart_GiveRoyalFavor.faction,
					amount = questPart_GiveRoyalFavor.amount
				});
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x0400756B RID: 30059
		public SlateRef<Pawn> giveTo;

		// Token: 0x0400756C RID: 30060
		public SlateRef<bool> giveToAccepter;

		// Token: 0x0400756D RID: 30061
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400756E RID: 30062
		public SlateRef<Faction> faction;

		// Token: 0x0400756F RID: 30063
		public SlateRef<Thing> factionOf;

		// Token: 0x04007570 RID: 30064
		public SlateRef<int> amount;

		// Token: 0x04007571 RID: 30065
		public SlateRef<bool> isSingleReward;
	}
}
