using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE3 RID: 7907
	public class QuestNode_Greater : QuestNode
	{
		// Token: 0x0600A9A2 RID: 43426 RVA: 0x00318E0C File Offset: 0x0031700C
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9A3 RID: 43427 RVA: 0x00318E60 File Offset: 0x00317060
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

		// Token: 0x040072E4 RID: 29412
		public SlateRef<double> value1;

		// Token: 0x040072E5 RID: 29413
		public SlateRef<double> value2;

		// Token: 0x040072E6 RID: 29414
		public QuestNode node;

		// Token: 0x040072E7 RID: 29415
		public QuestNode elseNode;
	}
}
