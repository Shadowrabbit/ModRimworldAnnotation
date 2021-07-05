using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C5 RID: 5829
	public class QuestNode_GiveRoyalFavor : QuestNode
	{
		// Token: 0x0600870A RID: 34570 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600870B RID: 34571 RVA: 0x003058E0 File Offset: 0x00303AE0
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

		// Token: 0x040054F8 RID: 21752
		public SlateRef<Pawn> giveTo;

		// Token: 0x040054F9 RID: 21753
		public SlateRef<bool> giveToAccepter;

		// Token: 0x040054FA RID: 21754
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054FB RID: 21755
		public SlateRef<Faction> faction;

		// Token: 0x040054FC RID: 21756
		public SlateRef<Thing> factionOf;

		// Token: 0x040054FD RID: 21757
		public SlateRef<int> amount;

		// Token: 0x040054FE RID: 21758
		public SlateRef<bool> isSingleReward;
	}
}
