using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001628 RID: 5672
	public class QuestNode_GreaterOrEqual : QuestNode
	{
		// Token: 0x060084DC RID: 34012 RVA: 0x002FBC94 File Offset: 0x002F9E94
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084DD RID: 34013 RVA: 0x002FBCE8 File Offset: 0x002F9EE8
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

		// Token: 0x04005291 RID: 21137
		public SlateRef<double> value1;

		// Token: 0x04005292 RID: 21138
		public SlateRef<double> value2;

		// Token: 0x04005293 RID: 21139
		public QuestNode node;

		// Token: 0x04005294 RID: 21140
		public QuestNode elseNode;
	}
}
