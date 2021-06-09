using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE5 RID: 7909
	public class QuestNode_GreaterOrFail : QuestNode
	{
		// Token: 0x0600A9A8 RID: 43432 RVA: 0x0006F600 File Offset: 0x0006D800
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) > this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600A9A9 RID: 43433 RVA: 0x00318F5C File Offset: 0x0031715C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x040072EC RID: 29420
		public SlateRef<double> value1;

		// Token: 0x040072ED RID: 29421
		public SlateRef<double> value2;

		// Token: 0x040072EE RID: 29422
		public QuestNode node;
	}
}
