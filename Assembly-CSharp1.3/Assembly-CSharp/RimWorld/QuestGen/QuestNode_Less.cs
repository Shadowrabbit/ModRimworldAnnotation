using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001630 RID: 5680
	public class QuestNode_Less : QuestNode
	{
		// Token: 0x060084F4 RID: 34036 RVA: 0x002FC174 File Offset: 0x002FA374
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084F5 RID: 34037 RVA: 0x002FC1C8 File Offset: 0x002FA3C8
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

		// Token: 0x040052AB RID: 21163
		public SlateRef<double> value1;

		// Token: 0x040052AC RID: 21164
		public SlateRef<double> value2;

		// Token: 0x040052AD RID: 21165
		public QuestNode node;

		// Token: 0x040052AE RID: 21166
		public QuestNode elseNode;
	}
}
