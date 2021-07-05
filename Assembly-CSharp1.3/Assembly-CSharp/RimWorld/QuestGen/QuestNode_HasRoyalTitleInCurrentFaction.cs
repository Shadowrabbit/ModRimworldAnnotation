using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001699 RID: 5785
	public class QuestNode_HasRoyalTitleInCurrentFaction : QuestNode
	{
		// Token: 0x06008674 RID: 34420 RVA: 0x003039C4 File Offset: 0x00301BC4
		protected override bool TestRunInt(Slate slate)
		{
			if (this.HasRoyalTitleInCurrentFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008675 RID: 34421 RVA: 0x003039FC File Offset: 0x00301BFC
		protected override void RunInt()
		{
			if (this.HasRoyalTitleInCurrentFaction(QuestGen.slate))
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

		// Token: 0x06008676 RID: 34422 RVA: 0x00303A34 File Offset: 0x00301C34
		private bool HasRoyalTitleInCurrentFaction(Slate slate)
		{
			Pawn value = this.pawn.GetValue(slate);
			return value != null && value.Faction != null && value.royalty != null && value.royalty.HasAnyTitleIn(value.Faction);
		}

		// Token: 0x04005451 RID: 21585
		public SlateRef<Pawn> pawn;

		// Token: 0x04005452 RID: 21586
		public QuestNode node;

		// Token: 0x04005453 RID: 21587
		public QuestNode elseNode;
	}
}
