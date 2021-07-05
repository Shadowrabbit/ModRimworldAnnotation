using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EFA RID: 7930
	public class QuestNode_RandomNode : QuestNode
	{
		// Token: 0x0600A9E8 RID: 43496 RVA: 0x0031A070 File Offset: 0x00318270
		protected override bool TestRunInt(Slate slate)
		{
			QuestNode questNode = this.GetNodesCanRun(slate).FirstOrDefault<QuestNode>();
			if (questNode == null)
			{
				return false;
			}
			questNode.TestRun(slate);
			return true;
		}

		// Token: 0x0600A9E9 RID: 43497 RVA: 0x0031A098 File Offset: 0x00318298
		protected override void RunInt()
		{
			QuestNode questNode;
			if (!this.GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out questNode))
			{
				return;
			}
			questNode.Run();
		}

		// Token: 0x0600A9EA RID: 43498 RVA: 0x0006F787 File Offset: 0x0006D987
		private IEnumerable<QuestNode> GetNodesCanRun(Slate slate)
		{
			int num;
			for (int i = 0; i < this.nodes.Count; i = num + 1)
			{
				if (this.nodes[i].SelectionWeight(slate) > 0f && this.nodes[i].TestRun(slate.DeepCopy()))
				{
					yield return this.nodes[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400733D RID: 29501
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
