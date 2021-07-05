using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200169E RID: 5790
	public class QuestNode_IsOfRoyalFaction : QuestNode
	{
		// Token: 0x06008688 RID: 34440 RVA: 0x00303D34 File Offset: 0x00301F34
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfRoyalFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008689 RID: 34441 RVA: 0x00303D6C File Offset: 0x00301F6C
		protected override void RunInt()
		{
			if (this.IsOfRoyalFaction(QuestGen.slate))
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

		// Token: 0x0600868A RID: 34442 RVA: 0x00303DA2 File Offset: 0x00301FA2
		private bool IsOfRoyalFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction != null && this.thing.GetValue(slate).Faction.def.HasRoyalTitles;
		}

		// Token: 0x04005462 RID: 21602
		public SlateRef<Thing> thing;

		// Token: 0x04005463 RID: 21603
		public QuestNode node;

		// Token: 0x04005464 RID: 21604
		public QuestNode elseNode;
	}
}
