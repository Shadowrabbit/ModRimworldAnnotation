using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC2 RID: 8130
	public class QuestNode_SituationalThought : QuestNode
	{
		// Token: 0x0600AC8F RID: 44175 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC90 RID: 44176 RVA: 0x00322834 File Offset: 0x00320A34
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SituationalThought questPart_SituationalThought = new QuestPart_SituationalThought();
			questPart_SituationalThought.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SituationalThought.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_SituationalThought.def = this.def.GetValue(slate);
			questPart_SituationalThought.pawn = this.pawn.GetValue(slate);
			questPart_SituationalThought.delayTicks = this.delayTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SituationalThought);
		}

		// Token: 0x04007606 RID: 30214
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007607 RID: 30215
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04007608 RID: 30216
		public SlateRef<ThoughtDef> def;

		// Token: 0x04007609 RID: 30217
		public SlateRef<Pawn> pawn;

		// Token: 0x0400760A RID: 30218
		public SlateRef<int> delayTicks;
	}
}
