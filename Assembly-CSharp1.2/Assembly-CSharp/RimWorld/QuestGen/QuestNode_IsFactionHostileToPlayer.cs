using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F6D RID: 8045
	public class QuestNode_IsFactionHostileToPlayer : QuestNode
	{
		// Token: 0x0600AB78 RID: 43896 RVA: 0x000702ED File Offset: 0x0006E4ED
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsHostile(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB79 RID: 43897 RVA: 0x00070325 File Offset: 0x0006E525
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

		// Token: 0x0600AB7A RID: 43898 RVA: 0x0031F7D8 File Offset: 0x0031D9D8
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

		// Token: 0x040074D7 RID: 29911
		public SlateRef<Faction> faction;

		// Token: 0x040074D8 RID: 29912
		public SlateRef<Thing> factionOf;

		// Token: 0x040074D9 RID: 29913
		public QuestNode node;

		// Token: 0x040074DA RID: 29914
		public QuestNode elseNode;
	}
}
