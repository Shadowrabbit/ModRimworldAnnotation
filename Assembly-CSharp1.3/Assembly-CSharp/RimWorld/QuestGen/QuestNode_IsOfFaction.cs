using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200169D RID: 5789
	public class QuestNode_IsOfFaction : QuestNode
	{
		// Token: 0x06008684 RID: 34436 RVA: 0x00303C95 File Offset: 0x00301E95
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008685 RID: 34437 RVA: 0x00303CCD File Offset: 0x00301ECD
		protected override void RunInt()
		{
			if (this.IsOfFaction(QuestGen.slate))
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

		// Token: 0x06008686 RID: 34438 RVA: 0x00303D03 File Offset: 0x00301F03
		private bool IsOfFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction == this.faction.GetValue(slate);
		}

		// Token: 0x0400545E RID: 21598
		public SlateRef<Thing> thing;

		// Token: 0x0400545F RID: 21599
		public SlateRef<Faction> faction;

		// Token: 0x04005460 RID: 21600
		public QuestNode node;

		// Token: 0x04005461 RID: 21601
		public QuestNode elseNode;
	}
}
