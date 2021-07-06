using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F89 RID: 8073
	public class QuestNode_ChangeNeed : QuestNode
	{
		// Token: 0x0600ABD4 RID: 43988 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABD5 RID: 43989 RVA: 0x00320300 File Offset: 0x0031E500
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

		// Token: 0x04007521 RID: 29985
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007522 RID: 29986
		public SlateRef<Pawn> pawn;

		// Token: 0x04007523 RID: 29987
		public SlateRef<NeedDef> need;

		// Token: 0x04007524 RID: 29988
		public SlateRef<float> offset;
	}
}
