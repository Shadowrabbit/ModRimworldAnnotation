using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EEF RID: 7919
	public class QuestNode_AddRangeToList : QuestNode
	{
		// Token: 0x0600A9C6 RID: 43462 RVA: 0x00319478 File Offset: 0x00317678
		protected override bool TestRunInt(Slate slate)
		{
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
			return true;
		}

		// Token: 0x0600A9C7 RID: 43463 RVA: 0x003194AC File Offset: 0x003176AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
		}

		// Token: 0x0400730D RID: 29453
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400730E RID: 29454
		public SlateRef<List<object>> value;
	}
}
