using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE7 RID: 7911
	public class QuestNode_IsNull : QuestNode
	{
		// Token: 0x0600A9AE RID: 43438 RVA: 0x0006F634 File Offset: 0x0006D834
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == null)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9AF RID: 43439 RVA: 0x00319054 File Offset: 0x00317254
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == null)
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

		// Token: 0x040072F3 RID: 29427
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x040072F4 RID: 29428
		public QuestNode node;

		// Token: 0x040072F5 RID: 29429
		public QuestNode elseNode;
	}
}
