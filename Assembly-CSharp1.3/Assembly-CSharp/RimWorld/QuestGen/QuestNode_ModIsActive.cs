using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A0 RID: 5792
	public class QuestNode_ModIsActive : QuestNode
	{
		// Token: 0x06008690 RID: 34448 RVA: 0x00303E88 File Offset: 0x00302088
		protected override bool TestRunInt(Slate slate)
		{
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008691 RID: 34449 RVA: 0x00303ED5 File Offset: 0x003020D5
		protected override void RunInt()
		{
			if (ModsConfig.IsActive(this.modID.GetValue(QuestGen.slate)))
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

		// Token: 0x04005468 RID: 21608
		public SlateRef<string> modID;

		// Token: 0x04005469 RID: 21609
		public QuestNode node;

		// Token: 0x0400546A RID: 21610
		public QuestNode elseNode;
	}
}
