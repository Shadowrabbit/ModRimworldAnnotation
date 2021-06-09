using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE9 RID: 7913
	public class QuestNode_IsTrue : QuestNode
	{
		// Token: 0x0600A9B4 RID: 43444 RVA: 0x0006F671 File Offset: 0x0006D871
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9B5 RID: 43445 RVA: 0x00319140 File Offset: 0x00317340
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

		// Token: 0x040072F9 RID: 29433
		public SlateRef<bool> value;

		// Token: 0x040072FA RID: 29434
		public QuestNode node;

		// Token: 0x040072FB RID: 29435
		public QuestNode elseNode;
	}
}
