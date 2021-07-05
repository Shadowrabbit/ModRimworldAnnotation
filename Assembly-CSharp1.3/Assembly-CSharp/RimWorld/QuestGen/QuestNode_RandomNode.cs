using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001640 RID: 5696
	public class QuestNode_RandomNode : QuestNode
	{
		// Token: 0x06008525 RID: 34085 RVA: 0x002FD338 File Offset: 0x002FB538
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

		// Token: 0x06008526 RID: 34086 RVA: 0x002FD360 File Offset: 0x002FB560
		protected override void RunInt()
		{
			QuestNode questNode;
			if (!this.GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out questNode))
			{
				return;
			}
			questNode.Run();
		}

		// Token: 0x06008527 RID: 34087 RVA: 0x002FD3A7 File Offset: 0x002FB5A7
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

		// Token: 0x040052F3 RID: 21235
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
