using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200169A RID: 5786
	public class QuestNode_IsFactionHostileToPlayer : QuestNode
	{
		// Token: 0x06008678 RID: 34424 RVA: 0x00303A74 File Offset: 0x00301C74
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsHostile(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008679 RID: 34425 RVA: 0x00303AAC File Offset: 0x00301CAC
		protected override void RunInt()
		{
			if (this.IsHostile(QuestGen.slate))
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

		// Token: 0x0600867A RID: 34426 RVA: 0x00303AE4 File Offset: 0x00301CE4
		private bool IsHostile(Slate slate)
		{
			Faction value = this.faction.GetValue(slate);
			if (value != null)
			{
				return value.HostileTo(Faction.OfPlayer);
			}
			Thing value2 = this.factionOf.GetValue(slate);
			return value2 != null && value2.Faction != null && value2.Faction.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x04005454 RID: 21588
		public SlateRef<Faction> faction;

		// Token: 0x04005455 RID: 21589
		public SlateRef<Thing> factionOf;

		// Token: 0x04005456 RID: 21590
		public QuestNode node;

		// Token: 0x04005457 RID: 21591
		public QuestNode elseNode;
	}
}
