using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001627 RID: 5671
	public class QuestNode_Greater : QuestNode
	{
		// Token: 0x060084D9 RID: 34009 RVA: 0x002FBBEC File Offset: 0x002F9DEC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084DA RID: 34010 RVA: 0x002FBC40 File Offset: 0x002F9E40
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
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

		// Token: 0x0400528D RID: 21133
		public SlateRef<double> value1;

		// Token: 0x0400528E RID: 21134
		public SlateRef<double> value2;

		// Token: 0x0400528F RID: 21135
		public QuestNode node;

		// Token: 0x04005290 RID: 21136
		public QuestNode elseNode;
	}
}
