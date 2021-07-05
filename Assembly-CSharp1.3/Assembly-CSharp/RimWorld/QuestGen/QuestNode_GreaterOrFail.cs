using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001629 RID: 5673
	public class QuestNode_GreaterOrFail : QuestNode
	{
		// Token: 0x060084DF RID: 34015 RVA: 0x002FBD3C File Offset: 0x002F9F3C
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) > this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x060084E0 RID: 34016 RVA: 0x002FBD70 File Offset: 0x002F9F70
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04005295 RID: 21141
		public SlateRef<double> value1;

		// Token: 0x04005296 RID: 21142
		public SlateRef<double> value2;

		// Token: 0x04005297 RID: 21143
		public QuestNode node;
	}
}
