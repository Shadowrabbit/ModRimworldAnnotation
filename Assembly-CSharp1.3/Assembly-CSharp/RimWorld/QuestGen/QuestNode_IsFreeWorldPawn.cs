using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200169C RID: 5788
	public class QuestNode_IsFreeWorldPawn : QuestNode
	{
		// Token: 0x06008680 RID: 34432 RVA: 0x00303BFC File Offset: 0x00301DFC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFreeWorldPawn(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x00303C34 File Offset: 0x00301E34
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

		// Token: 0x06008682 RID: 34434 RVA: 0x00303C6A File Offset: 0x00301E6A
		private bool IsFreeWorldPawn(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && Find.WorldPawns.GetSituation(this.pawn.GetValue(slate)) == WorldPawnSituation.Free;
		}

		// Token: 0x0400545B RID: 21595
		public SlateRef<Pawn> pawn;

		// Token: 0x0400545C RID: 21596
		public QuestNode node;

		// Token: 0x0400545D RID: 21597
		public QuestNode elseNode;
	}
}
