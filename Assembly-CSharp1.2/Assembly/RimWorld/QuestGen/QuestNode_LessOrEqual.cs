using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EED RID: 7917
	public class QuestNode_LessOrEqual : QuestNode
	{
		// Token: 0x0600A9C0 RID: 43456 RVA: 0x00319390 File Offset: 0x00317590
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9C1 RID: 43457 RVA: 0x003193E4 File Offset: 0x003175E4
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

		// Token: 0x04007306 RID: 29446
		public SlateRef<double> value1;

		// Token: 0x04007307 RID: 29447
		public SlateRef<double> value2;

		// Token: 0x04007308 RID: 29448
		public QuestNode node;

		// Token: 0x04007309 RID: 29449
		public QuestNode elseNode;
	}
}
