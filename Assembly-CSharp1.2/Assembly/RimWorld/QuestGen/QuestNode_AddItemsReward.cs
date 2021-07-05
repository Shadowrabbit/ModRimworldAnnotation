using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F7E RID: 8062
	public class QuestNode_AddItemsReward : QuestNode
	{
		// Token: 0x0600ABB2 RID: 43954 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABB3 RID: 43955 RVA: 0x0031FCB0 File Offset: 0x0031DEB0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Thing> value = this.items.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			Reward_Items reward_Items = new Reward_Items();
			reward_Items.items.AddRange(value);
			choice.rewards.Add(reward_Items);
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x040074FD RID: 29949
		public SlateRef<IEnumerable<Thing>> items;

		// Token: 0x040074FE RID: 29950
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
