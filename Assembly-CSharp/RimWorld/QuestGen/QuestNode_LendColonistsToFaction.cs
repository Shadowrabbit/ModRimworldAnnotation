using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA2 RID: 8098
	public class QuestNode_LendColonistsToFaction : QuestNode
	{
		// Token: 0x0600AC26 RID: 44070 RVA: 0x0032120C File Offset: 0x0031F40C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = new QuestPart_LendColonistsToFaction
			{
				inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shuttle = this.shuttle.GetValue(slate),
				lendColonistsToFaction = this.lendColonistsToFactionOf.GetValue(slate).Faction,
				returnLentColonistsInTicks = this.returnLentColonistsInTicks.GetValue(slate),
				returnMap = slate.Get<Map>("map", null, false).Parent
			};
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_LendColonistsToFaction.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_LendColonistsToFaction);
		}

		// Token: 0x0600AC27 RID: 44071 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04007586 RID: 30086
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007587 RID: 30087
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04007588 RID: 30088
		public SlateRef<Thing> shuttle;

		// Token: 0x04007589 RID: 30089
		public SlateRef<Pawn> lendColonistsToFactionOf;

		// Token: 0x0400758A RID: 30090
		public SlateRef<int> returnLentColonistsInTicks;
	}
}
