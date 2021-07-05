using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016CD RID: 5837
	public class QuestNode_Letter : QuestNode
	{
		// Token: 0x06008723 RID: 34595 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008724 RID: 34596 RVA: 0x00305FF8 File Offset: 0x003041F8
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
			questPart_Letter.acceptedVisitorsSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.acceptedVisitorsSignal.GetValue(slate));
			questPart_Letter.visitors = this.visitors.GetValue(slate);
			questPart_Letter.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			questPart_Letter.filterDeadPawnsFromLookTargets = this.filterDeadPawnsFromLookTargets.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Letter);
		}

		// Token: 0x0400551D RID: 21789
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400551E RID: 21790
		public SlateRef<Faction> relatedFaction;

		// Token: 0x0400551F RID: 21791
		public SlateRef<LetterDef> letterDef;

		// Token: 0x04005520 RID: 21792
		public SlateRef<string> label;

		// Token: 0x04005521 RID: 21793
		public SlateRef<string> text;

		// Token: 0x04005522 RID: 21794
		public SlateRef<RulePack> labelRules;

		// Token: 0x04005523 RID: 21795
		public SlateRef<RulePack> textRules;

		// Token: 0x04005524 RID: 21796
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04005525 RID: 21797
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04005526 RID: 21798
		[NoTranslate]
		public SlateRef<string> chosenPawnSignal;

		// Token: 0x04005527 RID: 21799
		public SlateRef<MapParent> useColonistsOnMap;

		// Token: 0x04005528 RID: 21800
		public SlateRef<bool> useColonistsFromCaravanArg;

		// Token: 0x04005529 RID: 21801
		[NoTranslate]
		public SlateRef<string> acceptedVisitorsSignal;

		// Token: 0x0400552A RID: 21802
		public SlateRef<List<Pawn>> visitors;

		// Token: 0x0400552B RID: 21803
		public SlateRef<bool> filterDeadPawnsFromLookTargets;

		// Token: 0x0400552C RID: 21804
		private const string RootSymbol = "root";
	}
}
