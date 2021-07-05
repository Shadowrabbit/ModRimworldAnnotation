using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016AA RID: 5802
	public class QuestNode_AddItemsReward : QuestNode
	{
		// Token: 0x060086B0 RID: 34480 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086B1 RID: 34481 RVA: 0x003043AC File Offset: 0x003025AC
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

		// Token: 0x0400547A RID: 21626
		public SlateRef<IEnumerable<Thing>> items;

		// Token: 0x0400547B RID: 21627
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
