using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF4 RID: 7924
	public class QuestNode_Chance : QuestNode
	{
		// Token: 0x0600A9D5 RID: 43477 RVA: 0x00319A1C File Offset: 0x00317C1C
		protected override bool TestRunInt(Slate slate)
		{
			if (this.node == null || this.elseNode == null)
			{
				return true;
			}
			if (this.node.TestRun(slate.DeepCopy()))
			{
				this.node.TestRun(slate);
				return true;
			}
			if (this.elseNode.TestRun(slate.DeepCopy()))
			{
				this.elseNode.TestRun(slate);
				return true;
			}
			return false;
		}

		// Token: 0x0600A9D6 RID: 43478 RVA: 0x00319A80 File Offset: 0x00317C80
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (Rand.Chance(this.chance.GetValue(slate)))
			{
				if (this.node == null)
				{
					return;
				}
				if (this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
					return;
				}
				if (this.elseNode != null && this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
			}
			else
			{
				if (this.elseNode == null)
				{
					return;
				}
				if (this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
				if (this.node != null && this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
				}
			}
		}

		// Token: 0x0400731D RID: 29469
		public SlateRef<float> chance;

		// Token: 0x0400731E RID: 29470
		public QuestNode node;

		// Token: 0x0400731F RID: 29471
		public QuestNode elseNode;
	}
}
