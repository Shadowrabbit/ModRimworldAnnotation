using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001631 RID: 5681
	public class QuestNode_LessOrEqual : QuestNode
	{
		// Token: 0x060084F7 RID: 34039 RVA: 0x002FC21C File Offset: 0x002FA41C
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084F8 RID: 34040 RVA: 0x002FC270 File Offset: 0x002FA470
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
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

		// Token: 0x040052AF RID: 21167
		public SlateRef<double> value1;

		// Token: 0x040052B0 RID: 21168
		public SlateRef<double> value2;

		// Token: 0x040052B1 RID: 21169
		public QuestNode node;

		// Token: 0x040052B2 RID: 21170
		public QuestNode elseNode;
	}
}
