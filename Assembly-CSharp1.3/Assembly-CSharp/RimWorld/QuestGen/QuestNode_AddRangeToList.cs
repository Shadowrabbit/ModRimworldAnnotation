using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001633 RID: 5683
	public class QuestNode_AddRangeToList : QuestNode
	{
		// Token: 0x060084FD RID: 34045 RVA: 0x002FC338 File Offset: 0x002FA538
		protected override bool TestRunInt(Slate slate)
		{
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
			return true;
		}

		// Token: 0x060084FE RID: 34046 RVA: 0x002FC36C File Offset: 0x002FA56C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
		}

		// Token: 0x040052B6 RID: 21174
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040052B7 RID: 21175
		public SlateRef<List<object>> value;
	}
}
