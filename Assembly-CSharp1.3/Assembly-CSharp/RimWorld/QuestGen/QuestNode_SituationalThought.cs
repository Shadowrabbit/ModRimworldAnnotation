using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E8 RID: 5864
	public class QuestNode_SituationalThought : QuestNode
	{
		// Token: 0x06008778 RID: 34680 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x003074E8 File Offset: 0x003056E8
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

		// Token: 0x0400558D RID: 21901
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400558E RID: 21902
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400558F RID: 21903
		public SlateRef<ThoughtDef> def;

		// Token: 0x04005590 RID: 21904
		public SlateRef<Pawn> pawn;

		// Token: 0x04005591 RID: 21905
		public SlateRef<int> delayTicks;
	}
}
