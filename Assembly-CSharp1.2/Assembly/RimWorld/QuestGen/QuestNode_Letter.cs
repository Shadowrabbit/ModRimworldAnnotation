using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA3 RID: 8099
	public class QuestNode_Letter : QuestNode
	{
		// Token: 0x0600AC29 RID: 44073 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC2A RID: 44074 RVA: 0x003212E0 File Offset: 0x0031F4E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Letter questPart_Letter = new QuestPart_Letter();
			questPart_Letter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			LetterDef letterDef = this.letterDef.GetValue(slate) ?? LetterDefOf.NeutralEvent;
			if (typeof(ChoiceLetter).IsAssignableFrom(letterDef.letterClass))
			{
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("error", "error", letterDef, QuestGenUtility.ToLookTargets(this.lookTargets, slate), this.relatedFaction.GetValue(slate), QuestGen.quest, null);
				questPart_Letter.letter = choiceLetter;
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.label = x;
				}, QuestGenUtility.MergeRules(this.labelRules.GetValue(slate), this.label.GetValue(slate), "root"));
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.text = x;
				}, QuestGenUtility.MergeRules(this.textRules.GetValue(slate), this.text.GetValue(slate), "root"));
			}
			else
			{
				questPart_Letter.letter = LetterMaker.MakeLetter(letterDef);
				questPart_Letter.letter.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
				questPart_Letter.letter.relatedFaction = this.relatedFaction.GetValue(slate);
			}
			questPart_Letter.chosenPawnSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.chosenPawnSignal.GetValue(slate));
			questPart_Letter.useColonistsOnMap = this.useColonistsOnMap.GetValue(slate);
			questPart_Letter.useColonistsFromCaravanArg = this.useColonistsFromCaravanArg.GetValue(slate);
			questPart_Letter.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			questPart_Letter.filterDeadPawnsFromLookTargets = this.filterDeadPawnsFromLookTargets.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Letter);
		}

		// Token: 0x0400758B RID: 30091
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400758C RID: 30092
		public SlateRef<Faction> relatedFaction;

		// Token: 0x0400758D RID: 30093
		public SlateRef<LetterDef> letterDef;

		// Token: 0x0400758E RID: 30094
		public SlateRef<string> label;

		// Token: 0x0400758F RID: 30095
		public SlateRef<string> text;

		// Token: 0x04007590 RID: 30096
		public SlateRef<RulePack> labelRules;

		// Token: 0x04007591 RID: 30097
		public SlateRef<RulePack> textRules;

		// Token: 0x04007592 RID: 30098
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04007593 RID: 30099
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04007594 RID: 30100
		[NoTranslate]
		public SlateRef<string> chosenPawnSignal;

		// Token: 0x04007595 RID: 30101
		public SlateRef<MapParent> useColonistsOnMap;

		// Token: 0x04007596 RID: 30102
		public SlateRef<bool> useColonistsFromCaravanArg;

		// Token: 0x04007597 RID: 30103
		public SlateRef<bool> filterDeadPawnsFromLookTargets;

		// Token: 0x04007598 RID: 30104
		private const string RootSymbol = "root";
	}
}
