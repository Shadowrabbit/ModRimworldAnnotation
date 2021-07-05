using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D0 RID: 5840
	public class QuestNode_Message : QuestNode
	{
		// Token: 0x0600872C RID: 34604 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600872D RID: 34605 RVA: 0x00306344 File Offset: 0x00304544
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Message message = new QuestPart_Message();
			message.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			message.messageType = (this.messageType.GetValue(slate) ?? MessageTypeDefOf.NeutralEvent);
			message.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				message.message = x;
			}, QuestGenUtility.MergeRules(this.rules.GetValue(slate), this.text.GetValue(slate), "root"));
			QuestGen.quest.AddPart(message);
		}

		// Token: 0x04005532 RID: 21810
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005533 RID: 21811
		public SlateRef<MessageTypeDef> messageType;

		// Token: 0x04005534 RID: 21812
		public SlateRef<string> text;

		// Token: 0x04005535 RID: 21813
		public SlateRef<RulePack> rules;

		// Token: 0x04005536 RID: 21814
		[NoTranslate]
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04005537 RID: 21815
		private const string RootSymbol = "root";
	}
}
