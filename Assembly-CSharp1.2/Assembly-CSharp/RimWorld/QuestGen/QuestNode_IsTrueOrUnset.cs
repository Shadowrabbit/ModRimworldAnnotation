using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EEA RID: 7914
	public class QuestNode_IsTrueOrUnset : QuestNode
	{
		// Token: 0x0600A9B7 RID: 43447 RVA: 0x00319188 File Offset: 0x00317388
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) ?? true)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9B8 RID: 43448 RVA: 0x003191E4 File Offset: 0x003173E4
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

		// Token: 0x040072FC RID: 29436
		public SlateRef<bool?> value;

		// Token: 0x040072FD RID: 29437
		public QuestNode node;

		// Token: 0x040072FE RID: 29438
		public QuestNode elseNode;
	}
}
