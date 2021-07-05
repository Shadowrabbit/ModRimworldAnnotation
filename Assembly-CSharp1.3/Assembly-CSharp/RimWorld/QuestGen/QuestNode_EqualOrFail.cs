using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001626 RID: 5670
	public class QuestNode_EqualOrFail : QuestNode
	{
		// Token: 0x060084D6 RID: 34006 RVA: 0x002FBB48 File Offset: 0x002F9D48
		protected override bool TestRunInt(Slate slate)
		{
			return QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x060084D7 RID: 34007 RVA: 0x002FBB98 File Offset: 0x002F9D98
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04005289 RID: 21129
		public SlateRef<object> value1;

		// Token: 0x0400528A RID: 21130
		public SlateRef<object> value2;

		// Token: 0x0400528B RID: 21131
		public SlateRef<Type> compareAs;

		// Token: 0x0400528C RID: 21132
		public QuestNode node;
	}
}
