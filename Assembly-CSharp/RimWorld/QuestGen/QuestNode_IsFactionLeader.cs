using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F6E RID: 8046
	public class QuestNode_IsFactionLeader : QuestNode
	{
		// Token: 0x0600AB7C RID: 43900 RVA: 0x0007035B File Offset: 0x0006E55B
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFactionLeader(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB7D RID: 43901 RVA: 0x00070393 File Offset: 0x0006E593
		protected override void RunInt()
		{
			if (this.IsFactionLeader(QuestGen.slate))
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

		// Token: 0x0600AB7E RID: 43902 RVA: 0x0031F82C File Offset: 0x0031DA2C
		private bool IsFactionLeader(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && this.pawn.GetValue(slate).Faction != null && this.pawn.GetValue(slate).Faction.leader == this.pawn.GetValue(slate);
		}

		// Token: 0x040074DB RID: 29915
		public SlateRef<Pawn> pawn;

		// Token: 0x040074DC RID: 29916
		public QuestNode node;

		// Token: 0x040074DD RID: 29917
		public QuestNode elseNode;
	}
}
