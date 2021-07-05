using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001624 RID: 5668
	public class QuestNode_Equal : QuestNode
	{
		// Token: 0x060084D2 RID: 34002 RVA: 0x002FB9FC File Offset: 0x002F9BFC
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084D3 RID: 34003 RVA: 0x002FBA64 File Offset: 0x002F9C64
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

		// Token: 0x04005284 RID: 21124
		[NoTranslate]
		public SlateRef<object> value1;

		// Token: 0x04005285 RID: 21125
		[NoTranslate]
		public SlateRef<object> value2;

		// Token: 0x04005286 RID: 21126
		public SlateRef<Type> compareAs;

		// Token: 0x04005287 RID: 21127
		public QuestNode node;

		// Token: 0x04005288 RID: 21128
		public QuestNode elseNode;
	}
}
