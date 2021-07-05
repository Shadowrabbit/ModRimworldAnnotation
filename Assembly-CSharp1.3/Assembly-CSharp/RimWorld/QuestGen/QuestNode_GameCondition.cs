using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200171C RID: 5916
	public class QuestNode_GameCondition : QuestNode
	{
		// Token: 0x0600887C RID: 34940 RVA: 0x00310754 File Offset: 0x0030E954
		private static Map GetMap(Slate slate)
		{
			Map randomPlayerHomeMap;
			if (!slate.TryGet<Map>("map", out randomPlayerHomeMap, false))
			{
				randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			}
			return randomPlayerHomeMap;
		}

		// Token: 0x0600887D RID: 34941 RVA: 0x00310778 File Offset: 0x0030E978
		protected override bool TestRunInt(Slate slate)
		{
			return this.targetWorld.GetValue(slate) || QuestNode_GameCondition.GetMap(slate) != null;
		}

		// Token: 0x0600887E RID: 34942 RVA: 0x00310794 File Offset: 0x0030E994
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.gameCondition.GetValue(slate), this.duration.GetValue(slate));
			QuestPart_GameCondition questPart_GameCondition = new QuestPart_GameCondition();
			questPart_GameCondition.gameCondition = gameCondition;
			List<Rule> list = new List<Rule>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this.targetWorld.GetValue(slate))
			{
				questPart_GameCondition.targetWorld = true;
				gameCondition.RandomizeSettings(points, null, list, dictionary);
			}
			else
			{
				Map map = QuestNode_GameCondition.GetMap(QuestGen.slate);
				questPart_GameCondition.mapParent = map.Parent;
				gameCondition.RandomizeSettings(points, map, list, dictionary);
			}
			questPart_GameCondition.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GameCondition);
			if (!this.storeGameConditionDescriptionFutureAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<string>(this.storeGameConditionDescriptionFutureAs.GetValue(slate), gameCondition.def.descriptionFuture, false);
			}
			QuestGen.AddQuestNameRules(list);
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestDescriptionConstants(dictionary);
		}

		// Token: 0x04005662 RID: 22114
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005663 RID: 22115
		public SlateRef<GameConditionDef> gameCondition;

		// Token: 0x04005664 RID: 22116
		public SlateRef<bool> targetWorld;

		// Token: 0x04005665 RID: 22117
		public SlateRef<int> duration;

		// Token: 0x04005666 RID: 22118
		[NoTranslate]
		public SlateRef<string> storeGameConditionDescriptionFutureAs;
	}
}
