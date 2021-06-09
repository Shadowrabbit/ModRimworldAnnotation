using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F6C RID: 8044
	public class QuestNode_HasRoyalTitleInCurrentFaction : QuestNode
	{
		// Token: 0x0600AB74 RID: 43892 RVA: 0x0007027F File Offset: 0x0006E47F
		protected override bool TestRunInt(Slate slate)
		{
			if (this.HasRoyalTitleInCurrentFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB75 RID: 43893 RVA: 0x000702B7 File Offset: 0x0006E4B7
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

		// Token: 0x0600AB76 RID: 43894 RVA: 0x0031F798 File Offset: 0x0031D998
		private bool HasRoyalTitleInCurrentFaction(Slate slate)
		{
			Pawn value = this.pawn.GetValue(slate);
			return value != null && value.Faction != null && value.royalty != null && value.royalty.HasAnyTitleIn(value.Faction);
		}

		// Token: 0x040074D4 RID: 29908
		public SlateRef<Pawn> pawn;

		// Token: 0x040074D5 RID: 29909
		public QuestNode node;

		// Token: 0x040074D6 RID: 29910
		public QuestNode elseNode;
	}
}
