using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001723 RID: 5923
	public class QuestNode_RandomRaid : QuestNode
	{
		// Token: 0x06008892 RID: 34962 RVA: 0x00311690 File Offset: 0x0030F890
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && (slate.Exists("enemyFaction", false) || this.faction.GetValue(slate) != null);
		}

		// Token: 0x06008893 RID: 34963 RVA: 0x003116E0 File Offset: 0x0030F8E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float val = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = this.faction.GetValue(slate) ?? QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_RandomRaid randomRaid = new QuestPart_RandomRaid();
			randomRaid.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			randomRaid.mapParent = map.Parent;
			randomRaid.faction = faction;
			randomRaid.pointsRange = QuestNode_RandomRaid.RaidPointsRandomFactor * val;
			randomRaid.useCurrentThreatPoints = this.useCurrentThreatPoints.GetValue(slate);
			randomRaid.currentThreatPointsFactor = (this.currentThreatPointsFactor.GetValue(slate) ?? 1f);
			if (this.arrivalMode.GetValue(slate) != null)
			{
				randomRaid.arrivalMode = this.arrivalMode.GetValue(slate);
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					randomRaid.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					randomRaid.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(randomRaid);
		}

		// Token: 0x04005695 RID: 22165
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005696 RID: 22166
		public SlateRef<Faction> faction;

		// Token: 0x04005697 RID: 22167
		public SlateRef<bool> useCurrentThreatPoints;

		// Token: 0x04005698 RID: 22168
		public SlateRef<float?> currentThreatPointsFactor;

		// Token: 0x04005699 RID: 22169
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x0400569A RID: 22170
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400569B RID: 22171
		public SlateRef<string> customLetterText;

		// Token: 0x0400569C RID: 22172
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400569D RID: 22173
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400569E RID: 22174
		private const string RootSymbol = "root";

		// Token: 0x0400569F RID: 22175
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
