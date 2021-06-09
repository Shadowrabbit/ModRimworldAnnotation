using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE2 RID: 7906
	public class QuestNode_EqualOrFail : QuestNode
	{
		// Token: 0x0600A99F RID: 43423 RVA: 0x00318D68 File Offset: 0x00316F68
		protected override bool TestRunInt(Slate slate)
		{
			return QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600A9A0 RID: 43424 RVA: 0x00318DB8 File Offset: 0x00316FB8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x040072E0 RID: 29408
		public SlateRef<object> value1;

		// Token: 0x040072E1 RID: 29409
		public SlateRef<object> value2;

		// Token: 0x040072E2 RID: 29410
		public SlateRef<Type> compareAs;

		// Token: 0x040072E3 RID: 29411
		public QuestNode node;
	}
}
