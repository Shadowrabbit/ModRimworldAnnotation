using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F6F RID: 8047
	public class QuestNode_IsFreeWorldPawn : QuestNode
	{
		// Token: 0x0600AB80 RID: 43904 RVA: 0x000703C9 File Offset: 0x0006E5C9
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFreeWorldPawn(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB81 RID: 43905 RVA: 0x00070401 File Offset: 0x0006E601
		protected override void RunInt()
		{
			if (this.IsFreeWorldPawn(QuestGen.slate))
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

		// Token: 0x0600AB82 RID: 43906 RVA: 0x00070437 File Offset: 0x0006E637
		private bool IsFreeWorldPawn(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && Find.WorldPawns.GetSituation(this.pawn.GetValue(slate)) == WorldPawnSituation.Free;
		}

		// Token: 0x040074DE RID: 29918
		public SlateRef<Pawn> pawn;

		// Token: 0x040074DF RID: 29919
		public QuestNode node;

		// Token: 0x040074E0 RID: 29920
		public QuestNode elseNode;
	}
}
