using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FAD RID: 8109
	public class QuestNode_PawnsKilled : QuestNode
	{
		// Token: 0x0600AC46 RID: 44102 RVA: 0x0007089A File Offset: 0x0006EA9A
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficultyValues.allowViolentQuests && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600AC47 RID: 44103 RVA: 0x00321B0C File Offset: 0x0031FD0C
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

		// Token: 0x040075BD RID: 30141
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040075BE RID: 30142
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040075BF RID: 30143
		[NoTranslate]
		public SlateRef<string> outSignalPawnsNotAvailable;

		// Token: 0x040075C0 RID: 30144
		public SlateRef<ThingDef> race;

		// Token: 0x040075C1 RID: 30145
		public SlateRef<int> count;

		// Token: 0x040075C2 RID: 30146
		public QuestNode node;

		// Token: 0x040075C3 RID: 30147
		private const string PawnOfRaceKilledSignal = "PawnOfRaceKilled";
	}
}
