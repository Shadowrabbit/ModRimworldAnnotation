using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA7 RID: 8103
	public class QuestNode_Message : QuestNode
	{
		// Token: 0x0600AC35 RID: 44085 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC36 RID: 44086 RVA: 0x00321600 File Offset: 0x0031F800
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

		// Token: 0x0400759F RID: 30111
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040075A0 RID: 30112
		public SlateRef<MessageTypeDef> messageType;

		// Token: 0x040075A1 RID: 30113
		public SlateRef<string> text;

		// Token: 0x040075A2 RID: 30114
		public SlateRef<RulePack> rules;

		// Token: 0x040075A3 RID: 30115
		[NoTranslate]
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x040075A4 RID: 30116
		private const string RootSymbol = "root";
	}
}
