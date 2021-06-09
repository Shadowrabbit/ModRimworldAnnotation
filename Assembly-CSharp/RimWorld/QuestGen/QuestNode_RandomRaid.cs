using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FFD RID: 8189
	public class QuestNode_RandomRaid : QuestNode
	{
		// Token: 0x0600AD78 RID: 44408 RVA: 0x00070E64 File Offset: 0x0006F064
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficultyValues.allowViolentQuests && slate.Exists("map", false) && slate.Exists("enemyFaction", false);
		}

		// Token: 0x0600AD79 RID: 44409 RVA: 0x00328114 File Offset: 0x00326314
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float val = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_RandomRaid questPart_RandomRaid = new QuestPart_RandomRaid();
			questPart_RandomRaid.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RandomRaid.mapParent = map.Parent;
			questPart_RandomRaid.faction = faction;
			questPart_RandomRaid.pointsRange = QuestNode_RandomRaid.RaidPointsRandomFactor * val;
			questPart_RandomRaid.useCurrentThreatPoints = this.useCurrentThreatPoints.GetValue(slate);
			QuestGen.quest.AddPart(questPart_RandomRaid);
		}

		// Token: 0x04007709 RID: 30473
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400770A RID: 30474
		public SlateRef<bool> useCurrentThreatPoints;

		// Token: 0x0400770B RID: 30475
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
