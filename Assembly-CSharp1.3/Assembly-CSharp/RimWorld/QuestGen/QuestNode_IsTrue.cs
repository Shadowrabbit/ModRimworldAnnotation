using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200162D RID: 5677
	public class QuestNode_IsTrue : QuestNode
	{
		// Token: 0x060084EB RID: 34027 RVA: 0x002FBF8F File Offset: 0x002FA18F
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084EC RID: 34028 RVA: 0x002FBFCC File Offset: 0x002FA1CC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate))
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

		// Token: 0x040052A2 RID: 21154
		public SlateRef<bool> value;

		// Token: 0x040052A3 RID: 21155
		public QuestNode node;

		// Token: 0x040052A4 RID: 21156
		public QuestNode elseNode;
	}
}
