using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B1 RID: 5809
	public class QuestNode_CannotRun : QuestNode
	{
		// Token: 0x060086C5 RID: 34501 RVA: 0x0000313F File Offset: 0x0000133F
		protected override void RunInt()
		{
		}

		// Token: 0x060086C6 RID: 34502 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool TestRunInt(Slate slate)
		{
			return false;
		}
	}
}
