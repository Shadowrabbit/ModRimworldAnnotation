using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE0 RID: 7904
	public class QuestNode_Equal : QuestNode
	{
		// Token: 0x0600A99B RID: 43419 RVA: 0x00318C1C File Offset: 0x00316E1C
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A99C RID: 43420 RVA: 0x00318C84 File Offset: 0x00316E84
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
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

		// Token: 0x040072DB RID: 29403
		[NoTranslate]
		public SlateRef<object> value1;

		// Token: 0x040072DC RID: 29404
		[NoTranslate]
		public SlateRef<object> value2;

		// Token: 0x040072DD RID: 29405
		public SlateRef<Type> compareAs;

		// Token: 0x040072DE RID: 29406
		public QuestNode node;

		// Token: 0x040072DF RID: 29407
		public QuestNode elseNode;
	}
}
