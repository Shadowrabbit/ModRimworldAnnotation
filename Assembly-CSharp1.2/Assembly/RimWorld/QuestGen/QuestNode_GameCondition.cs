using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FF0 RID: 8176
	public class QuestNode_GameCondition : QuestNode
	{
		// Token: 0x0600AD50 RID: 44368 RVA: 0x00327364 File Offset: 0x00325564
		private static Map GetMap(Slate slate)
		{
			Map randomPlayerHomeMap;
			if (!slate.TryGet<Map>("map", out randomPlayerHomeMap, false))
			{
				randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			}
			return randomPlayerHomeMap;
		}

		// Token: 0x0600AD51 RID: 44369 RVA: 0x00070DDD File Offset: 0x0006EFDD
		protected override bool TestRunInt(Slate slate)
		{
			return this.targetWorld.GetValue(slate) || QuestNode_GameCondition.GetMap(slate) != null;
		}

		// Token: 0x0600AD52 RID: 44370 RVA: 0x00327388 File Offset: 0x00325588
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

		// Token: 0x040076D2 RID: 30418
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076D3 RID: 30419
		public SlateRef<GameConditionDef> gameCondition;

		// Token: 0x040076D4 RID: 30420
		public SlateRef<bool> targetWorld;

		// Token: 0x040076D5 RID: 30421
		public SlateRef<int> duration;

		// Token: 0x040076D6 RID: 30422
		[NoTranslate]
		public SlateRef<string> storeGameConditionDescriptionFutureAs;
	}
}
