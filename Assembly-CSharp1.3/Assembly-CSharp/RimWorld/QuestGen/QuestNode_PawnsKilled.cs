using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D5 RID: 5845
	public class QuestNode_PawnsKilled : QuestNode
	{
		// Token: 0x0600873B RID: 34619 RVA: 0x00306920 File Offset: 0x00304B20
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600873C RID: 34620 RVA: 0x0030694C File Offset: 0x00304B4C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MapParent parent = slate.Get<Map>("map", null, false).Parent;
			string text = QuestGen.GenerateNewSignal("PawnOfRaceKilled", true);
			QuestPart_PawnsKilled questPart_PawnsKilled = new QuestPart_PawnsKilled();
			questPart_PawnsKilled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PawnsKilled.race = this.race.GetValue(slate);
			questPart_PawnsKilled.requiredInstigatorFaction = Faction.OfPlayer;
			questPart_PawnsKilled.count = this.count.GetValue(slate);
			questPart_PawnsKilled.mapParent = parent;
			questPart_PawnsKilled.outSignalPawnKilled = text;
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PawnsKilled);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PawnsKilled.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PawnsKilled);
			QuestPart_PawnsAvailable questPart_PawnsAvailable = new QuestPart_PawnsAvailable();
			questPart_PawnsAvailable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.outSignalPawnsNotAvailable.GetValue(slate).NullOrEmpty())
			{
				questPart_PawnsAvailable.outSignalPawnsNotAvailable = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalPawnsNotAvailable.GetValue(slate));
			}
			questPart_PawnsAvailable.race = this.race.GetValue(slate);
			questPart_PawnsAvailable.requiredCount = this.count.GetValue(slate);
			questPart_PawnsAvailable.mapParent = parent;
			questPart_PawnsAvailable.inSignalDecrement = text;
			QuestGen.quest.AddPart(questPart_PawnsAvailable);
		}

		// Token: 0x04005552 RID: 21842
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005553 RID: 21843
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04005554 RID: 21844
		[NoTranslate]
		public SlateRef<string> outSignalPawnsNotAvailable;

		// Token: 0x04005555 RID: 21845
		public SlateRef<ThingDef> race;

		// Token: 0x04005556 RID: 21846
		public SlateRef<int> count;

		// Token: 0x04005557 RID: 21847
		public QuestNode node;

		// Token: 0x04005558 RID: 21848
		private const string PawnOfRaceKilledSignal = "PawnOfRaceKilled";
	}
}
