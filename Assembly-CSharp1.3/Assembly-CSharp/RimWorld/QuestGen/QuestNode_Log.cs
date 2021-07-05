using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001649 RID: 5705
	public class QuestNode_Log : QuestNode
	{
		// Token: 0x06008543 RID: 34115 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008544 RID: 34116 RVA: 0x002FDC0B File Offset: 0x002FBE0B
		protected override void RunInt()
		{
			Log.Message("QuestNode_Log: " + this.message.ToString(QuestGen.slate));
		}

		// Token: 0x0400530F RID: 21263
		[NoTranslate]
		public SlateRef<object> message;
	}
}
