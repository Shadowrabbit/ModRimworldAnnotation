using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EEE RID: 7918
	public class QuestNode_LessOrFail : QuestNode
	{
		// Token: 0x0600A9C3 RID: 43459 RVA: 0x0006F6AE File Offset: 0x0006D8AE
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) < this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600A9C4 RID: 43460 RVA: 0x00319438 File Offset: 0x00317638
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x0400730A RID: 29450
		public SlateRef<double> value1;

		// Token: 0x0400730B RID: 29451
		public SlateRef<double> value2;

		// Token: 0x0400730C RID: 29452
		public QuestNode node;
	}
}
