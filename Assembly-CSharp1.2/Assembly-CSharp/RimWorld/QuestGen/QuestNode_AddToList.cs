using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF1 RID: 7921
	public class QuestNode_AddToList : QuestNode
	{
		// Token: 0x0600A9CC RID: 43468 RVA: 0x0006F6E2 File Offset: 0x0006D8E2
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.AddToOrMakeList(slate, this.name.GetValue(slate), this.value.GetValue(slate));
			return true;
		}

		// Token: 0x0600A9CD RID: 43469 RVA: 0x0031958C File Offset: 0x0031778C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.name.GetValue(slate), this.value.GetValue(slate));
		}

		// Token: 0x04007311 RID: 29457
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04007312 RID: 29458
		public SlateRef<object> value;
	}
}
