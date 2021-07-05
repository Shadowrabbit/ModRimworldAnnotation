using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200164B RID: 5707
	public class QuestNode_SlateDump : QuestNode
	{
		// Token: 0x06008549 RID: 34121 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600854A RID: 34122 RVA: 0x002FDC8E File Offset: 0x002FBE8E
		protected override void RunInt()
		{
			Log.Message(QuestGen.slate.ToString());
		}
	}
}
