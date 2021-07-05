using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200162E RID: 5678
	public class QuestNode_IsTrueOrUnset : QuestNode
	{
		// Token: 0x060084EE RID: 34030 RVA: 0x002FC014 File Offset: 0x002FA214
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) ?? true)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084EF RID: 34031 RVA: 0x002FC070 File Offset: 0x002FA270
		protected override void RunInt()
		{
			if (this.value.GetValue(QuestGen.slate) ?? true)
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

		// Token: 0x040052A5 RID: 21157
		public SlateRef<bool?> value;

		// Token: 0x040052A6 RID: 21158
		public QuestNode node;

		// Token: 0x040052A7 RID: 21159
		public QuestNode elseNode;
	}
}
