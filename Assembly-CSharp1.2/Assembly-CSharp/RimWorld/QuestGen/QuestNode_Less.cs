using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EEC RID: 7916
	public class QuestNode_Less : QuestNode
	{
		// Token: 0x0600A9BD RID: 43453 RVA: 0x003192E8 File Offset: 0x003174E8
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9BE RID: 43454 RVA: 0x0031933C File Offset: 0x0031753C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
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

		// Token: 0x04007302 RID: 29442
		public SlateRef<double> value1;

		// Token: 0x04007303 RID: 29443
		public SlateRef<double> value2;

		// Token: 0x04007304 RID: 29444
		public QuestNode node;

		// Token: 0x04007305 RID: 29445
		public QuestNode elseNode;
	}
}
