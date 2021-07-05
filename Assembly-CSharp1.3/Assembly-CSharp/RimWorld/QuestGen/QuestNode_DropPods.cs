using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016BD RID: 5821
	public class QuestNode_DropPods : QuestNode
	{
		// Token: 0x060086EA RID: 34538 RVA: 0x00304E1D File Offset: 0x0030301D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x060086EB RID: 34539 RVA: 0x00304EBC File Offset: 0x003030BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate).EnumerableNullOrEmpty<Thing>() && this.contentsDefs.GetValue(slate).EnumerableNullOrEmpty<ThingDefCountClass>())
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
			dropPods.dropSpot = (this.dropSpot.GetValue(slate) ?? IntVec3.Invalid);
			dropPods.joinPlayer = this.joinPlayer.GetValue(slate);
			dropPods.makePrisoners = this.makePrisoners.GetValue(slate);
			dropPods.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			dropPods.Things = this.contents.GetValue(slate);
			if (this.contentsDefs.GetValue(slate) != null)
			{
				dropPods.thingDefs.AddRange(this.contentsDefs.GetValue(slate));
			}
			if (this.thingsToExcludeFromHyperlinks.GetValue(slate) != null)
			{
				dropPods.thingsToExcludeFromHyperlinks.AddRange(from t in this.thingsToExcludeFromHyperlinks.GetValue(slate)
				select t.GetInnerIfMinified().def);
			}
			QuestGen.quest.AddPart(dropPods);
		}

		// Token: 0x040054B5 RID: 21685
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054B6 RID: 21686
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x040054B7 RID: 21687
		public SlateRef<IEnumerable<ThingDefCountClass>> contentsDefs;

		// Token: 0x040054B8 RID: 21688
		public SlateRef<IntVec3?> dropSpot;

		// Token: 0x040054B9 RID: 21689
		public SlateRef<bool> useTradeDropSpot;

		// Token: 0x040054BA RID: 21690
		public SlateRef<bool> joinPlayer;

		// Token: 0x040054BB RID: 21691
		public SlateRef<bool> makePrisoners;

		// Token: 0x040054BC RID: 21692
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x040054BD RID: 21693
		public SlateRef<string> customLetterLabel;

		// Token: 0x040054BE RID: 21694
		public SlateRef<string> customLetterText;

		// Token: 0x040054BF RID: 21695
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040054C0 RID: 21696
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040054C1 RID: 21697
		public SlateRef<IEnumerable<Thing>> thingsToExcludeFromHyperlinks;

		// Token: 0x040054C2 RID: 21698
		private const string RootSymbol = "root";
	}
}
