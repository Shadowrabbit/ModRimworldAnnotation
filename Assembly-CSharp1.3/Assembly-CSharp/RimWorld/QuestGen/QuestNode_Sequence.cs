using System;
using System.Collections.Generic;

namespace RimWorld.QuestGen
{
	// Token: 0x02001642 RID: 5698
	public class QuestNode_Sequence : QuestNode
	{
		// Token: 0x0600852C RID: 34092 RVA: 0x002FD538 File Offset: 0x002FB738
		protected override void RunInt()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].Run();
			}
		}

		// Token: 0x0600852D RID: 34093 RVA: 0x002FD56C File Offset: 0x002FB76C
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (!this.nodes[i].TestRun(slate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040052F7 RID: 21239
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
