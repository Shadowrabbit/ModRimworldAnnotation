using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EEB RID: 7915
	public class QuestNode_IsZero : QuestNode
	{
		// Token: 0x0600A9BA RID: 43450 RVA: 0x00319240 File Offset: 0x00317440
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == 0.0)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9BB RID: 43451 RVA: 0x00319294 File Offset: 0x00317494
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == 0.0)
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

		// Token: 0x040072FF RID: 29439
		public SlateRef<double> value;

		// Token: 0x04007300 RID: 29440
		public QuestNode node;

		// Token: 0x04007301 RID: 29441
		public QuestNode elseNode;
	}
}
