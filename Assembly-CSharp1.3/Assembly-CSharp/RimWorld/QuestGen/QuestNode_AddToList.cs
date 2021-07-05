using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001635 RID: 5685
	public class QuestNode_AddToList : QuestNode
	{
		// Token: 0x06008503 RID: 34051 RVA: 0x002FC464 File Offset: 0x002FA664
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.AddToOrMakeList(slate, this.name.GetValue(slate), this.value.GetValue(slate));
			return true;
		}

		// Token: 0x06008504 RID: 34052 RVA: 0x002FC488 File Offset: 0x002FA688
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.name.GetValue(slate), this.value.GetValue(slate));
		}

		// Token: 0x040052BA RID: 21178
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040052BB RID: 21179
		public SlateRef<object> value;
	}
}
