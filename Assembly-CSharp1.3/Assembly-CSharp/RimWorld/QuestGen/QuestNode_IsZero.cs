using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200162F RID: 5679
	public class QuestNode_IsZero : QuestNode
	{
		// Token: 0x060084F1 RID: 34033 RVA: 0x002FC0CC File Offset: 0x002FA2CC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == 0.0)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084F2 RID: 34034 RVA: 0x002FC120 File Offset: 0x002FA320
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == 0.0)
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x040052A8 RID: 21160
		public SlateRef<double> value;

		// Token: 0x040052A9 RID: 21161
		public QuestNode node;

		// Token: 0x040052AA RID: 21162
		public QuestNode elseNode;
	}
}
