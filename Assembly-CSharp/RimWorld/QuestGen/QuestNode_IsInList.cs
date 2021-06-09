using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE6 RID: 7910
	public class QuestNode_IsInList : QuestNode
	{
		// Token: 0x0600A9AB RID: 43435 RVA: 0x00318F9C File Offset: 0x0031719C
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9AC RID: 43436 RVA: 0x00318FF8 File Offset: 0x003171F8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
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

		// Token: 0x040072EF RID: 29423
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040072F0 RID: 29424
		public SlateRef<object> value;

		// Token: 0x040072F1 RID: 29425
		public QuestNode node;

		// Token: 0x040072F2 RID: 29426
		public QuestNode elseNode;
	}
}
