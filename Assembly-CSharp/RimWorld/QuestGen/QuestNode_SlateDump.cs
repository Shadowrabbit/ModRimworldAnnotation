using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F07 RID: 7943
	public class QuestNode_SlateDump : QuestNode
	{
		// Token: 0x0600AA16 RID: 43542 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA17 RID: 43543 RVA: 0x0006F8E4 File Offset: 0x0006DAE4
		protected override void RunInt()
		{
			Log.Message(QuestGen.slate.ToString(), false);
		}
	}
}
