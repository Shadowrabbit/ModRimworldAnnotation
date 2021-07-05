using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F90 RID: 8080
	public class QuestNode_DropPods : QuestNode
	{
		// Token: 0x0600ABE9 RID: 44009 RVA: 0x00070719 File Offset: 0x0006E919
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600ABEA RID: 44010 RVA: 0x0032066C File Offset: 0x0031E86C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_DropPods dropPods = new QuestPart_DropPods();
			dropPods.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			dropPods.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? dropPods.sendStandardLetter);
			dropPods.useTradeDropSpot = this.useTradeDropSpot.GetValue(slate);
			dropPods.joinPlayer = this.joinPlayer.GetValue(slate);
			dropPods.makePrisoners = this.makePrisoners.GetValue(slate);
			dropPods.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			dropPods.Things = this.contents.GetValue(slate);
			if (this.thingsToExcludeFromHyperlinks.GetValue(slate) != null)
			{
				dropPods.thingsToExcludeFromHyperlinks.AddRange(from t in this.thingsToExcludeFromHyperlinks.GetValue(slate)
				select t.GetInnerIfMinified().def);
			}
			QuestGen.quest.AddPart(dropPods);
		}

		// Token: 0x04007533 RID: 30003
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007534 RID: 30004
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x04007535 RID: 30005
		public SlateRef<bool> useTradeDropSpot;

		// Token: 0x04007536 RID: 30006
		public SlateRef<bool> joinPlayer;

		// Token: 0x04007537 RID: 30007
		public SlateRef<bool> makePrisoners;

		// Token: 0x04007538 RID: 30008
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04007539 RID: 30009
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400753A RID: 30010
		public SlateRef<string> customLetterText;

		// Token: 0x0400753B RID: 30011
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400753C RID: 30012
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400753D RID: 30013
		public SlateRef<IEnumerable<Thing>> thingsToExcludeFromHyperlinks;

		// Token: 0x0400753E RID: 30014
		private const string RootSymbol = "root";
	}
}
