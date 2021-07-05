using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001632 RID: 5682
	public class QuestNode_LessOrFail : QuestNode
	{
		// Token: 0x060084FA RID: 34042 RVA: 0x002FC2C4 File Offset: 0x002FA4C4
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) < this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x060084FB RID: 34043 RVA: 0x002FC2F8 File Offset: 0x002FA4F8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x040052B3 RID: 21171
		public SlateRef<double> value1;

		// Token: 0x040052B4 RID: 21172
		public SlateRef<double> value2;

		// Token: 0x040052B5 RID: 21173
		public QuestNode node;
	}
}
