using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE4 RID: 7908
	public class QuestNode_GreaterOrEqual : QuestNode
	{
		// Token: 0x0600A9A5 RID: 43429 RVA: 0x00318EB4 File Offset: 0x003170B4
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9A6 RID: 43430 RVA: 0x00318F08 File Offset: 0x00317108
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
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

		// Token: 0x040072E8 RID: 29416
		public SlateRef<double> value1;

		// Token: 0x040072E9 RID: 29417
		public SlateRef<double> value2;

		// Token: 0x040072EA RID: 29418
		public QuestNode node;

		// Token: 0x040072EB RID: 29419
		public QuestNode elseNode;
	}
}
