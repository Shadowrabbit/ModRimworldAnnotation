using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200169B RID: 5787
	public class QuestNode_IsFactionLeader : QuestNode
	{
		// Token: 0x0600867C RID: 34428 RVA: 0x00303B37 File Offset: 0x00301D37
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFactionLeader(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600867D RID: 34429 RVA: 0x00303B6F File Offset: 0x00301D6F
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

		// Token: 0x0600867E RID: 34430 RVA: 0x00303BA8 File Offset: 0x00301DA8
		private bool IsFactionLeader(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && this.pawn.GetValue(slate).Faction != null && this.pawn.GetValue(slate).Faction.leader == this.pawn.GetValue(slate);
		}

		// Token: 0x04005458 RID: 21592
		public SlateRef<Pawn> pawn;

		// Token: 0x04005459 RID: 21593
		public QuestNode node;

		// Token: 0x0400545A RID: 21594
		public QuestNode elseNode;
	}
}
