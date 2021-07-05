using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C3 RID: 5827
	public class QuestNode_GiveNearPawn : QuestNode
	{
		// Token: 0x060086FD RID: 34557 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086FE RID: 34558 RVA: 0x00305534 File Offset: 0x00303734
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate).EnumerableNullOrEmpty<Thing>() && this.contentsDefs.GetValue(slate).EnumerableNullOrEmpty<ThingDefCountClass>())
			{
				return;
			}
			QuestPart_GiveNearPawn give = new QuestPart_GiveNearPawn();
			give.nearPawn = this.nearPawn.GetValue(slate);
			give.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			if (!this.customDropPodsLetterLabel.GetValue(slate).NullOrEmpty() || this.customDropPodsLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customDropPodsLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customDropPodsLetterLabelRules.GetValue(slate), this.customDropPodsLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customDropPodsLetterText.GetValue(slate).NullOrEmpty() || this.customDropPodsLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customDropPodsLetterText = x;
				}, QuestGenUtility.MergeRules(this.customDropPodsLetterTextRules.GetValue(slate), this.customDropPodsLetterText.GetValue(slate), "root"));
			}
			if (!this.customCaravanInventoryLetterLabel.GetValue(slate).NullOrEmpty() || this.customCaravanInventoryLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customCaravanInventoryLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customCaravanInventoryLetterLabelRules.GetValue(slate), this.customCaravanInventoryLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customCaravanInventoryLetterText.GetValue(slate).NullOrEmpty() || this.customCaravanInventoryLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customCaravanInventoryLetterText = x;
				}, QuestGenUtility.MergeRules(this.customCaravanInventoryLetterTextRules.GetValue(slate), this.customCaravanInventoryLetterText.GetValue(slate), "root"));
			}
			give.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? give.sendStandardLetter);
			give.joinPlayer = this.joinPlayer.GetValue(slate);
			give.makePrisoners = this.makePrisoners.GetValue(slate);
			give.Things = this.contents.GetValue(slate);
			if (this.contentsDefs.GetValue(slate) != null)
			{
				give.thingDefs.AddRange(this.contentsDefs.GetValue(slate));
			}
			QuestGen.quest.AddPart(give);
		}

		// Token: 0x040054DB RID: 21723
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054DC RID: 21724
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x040054DD RID: 21725
		public SlateRef<IEnumerable<ThingDefCountClass>> contentsDefs;

		// Token: 0x040054DE RID: 21726
		public SlateRef<Pawn> nearPawn;

		// Token: 0x040054DF RID: 21727
		public SlateRef<bool> joinPlayer;

		// Token: 0x040054E0 RID: 21728
		public SlateRef<bool> makePrisoners;

		// Token: 0x040054E1 RID: 21729
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x040054E2 RID: 21730
		public SlateRef<string> customDropPodsLetterLabel;

		// Token: 0x040054E3 RID: 21731
		public SlateRef<string> customDropPodsLetterText;

		// Token: 0x040054E4 RID: 21732
		public SlateRef<string> customCaravanInventoryLetterLabel;

		// Token: 0x040054E5 RID: 21733
		public SlateRef<string> customCaravanInventoryLetterText;

		// Token: 0x040054E6 RID: 21734
		public SlateRef<RulePack> customDropPodsLetterLabelRules;

		// Token: 0x040054E7 RID: 21735
		public SlateRef<RulePack> customDropPodsLetterTextRules;

		// Token: 0x040054E8 RID: 21736
		public SlateRef<RulePack> customCaravanInventoryLetterLabelRules;

		// Token: 0x040054E9 RID: 21737
		public SlateRef<RulePack> customCaravanInventoryLetterTextRules;

		// Token: 0x040054EA RID: 21738
		private const string RootSymbol = "root";
	}
}
