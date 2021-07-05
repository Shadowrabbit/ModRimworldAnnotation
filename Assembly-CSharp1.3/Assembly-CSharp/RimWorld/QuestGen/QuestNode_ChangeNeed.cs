using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B6 RID: 5814
	public class QuestNode_ChangeNeed : QuestNode
	{
		// Token: 0x060086D5 RID: 34517 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086D6 RID: 34518 RVA: 0x00304B44 File Offset: 0x00302D44
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeNeed questPart_ChangeNeed = new QuestPart_ChangeNeed();
			questPart_ChangeNeed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeNeed.pawn = this.pawn.GetValue(slate);
			questPart_ChangeNeed.need = this.need.GetValue(slate);
			questPart_ChangeNeed.offset = this.offset.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeNeed);
		}

		// Token: 0x040054A3 RID: 21667
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054A4 RID: 21668
		public SlateRef<Pawn> pawn;

		// Token: 0x040054A5 RID: 21669
		public SlateRef<NeedDef> need;

		// Token: 0x040054A6 RID: 21670
		public SlateRef<float> offset;
	}
}
