using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200163A RID: 5690
	public class QuestNode_Chance : QuestNode
	{
		// Token: 0x06008512 RID: 34066 RVA: 0x002FCC70 File Offset: 0x002FAE70
		protected override bool TestRunInt(Slate slate)
		{
			if (this.node == null || this.elseNode == null)
			{
				return true;
			}
			if (this.node.TestRun(slate.DeepCopy()))
			{
				this.node.TestRun(slate);
				return true;
			}
			if (this.elseNode.TestRun(slate.DeepCopy()))
			{
				this.elseNode.TestRun(slate);
				return true;
			}
			return false;
		}

		// Token: 0x06008513 RID: 34067 RVA: 0x002FCCD4 File Offset: 0x002FAED4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (Rand.Chance(this.chance.GetValue(slate)))
			{
				if (this.node == null)
				{
					return;
				}
				if (this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
					return;
				}
				if (this.elseNode != null && this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
			}
			else
			{
				if (this.elseNode == null)
				{
					return;
				}
				if (this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
				if (this.node != null && this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
				}
			}
		}

		// Token: 0x040052D2 RID: 21202
		public SlateRef<float> chance;

		// Token: 0x040052D3 RID: 21203
		public QuestNode node;

		// Token: 0x040052D4 RID: 21204
		public QuestNode elseNode;
	}
}
