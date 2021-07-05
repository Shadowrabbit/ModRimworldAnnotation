using System;
using System.Collections.Generic;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EFE RID: 7934
	public class QuestNode_Sequence : QuestNode
	{
		// Token: 0x0600A9FA RID: 43514 RVA: 0x0031A35C File Offset: 0x0031855C
		protected override void RunInt()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].Run();
			}
		}

		// Token: 0x0600A9FB RID: 43515 RVA: 0x0031A390 File Offset: 0x00318590
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

		// Token: 0x0400734A RID: 29514
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
