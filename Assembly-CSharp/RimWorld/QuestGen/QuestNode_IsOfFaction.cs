using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F70 RID: 8048
	public class QuestNode_IsOfFaction : QuestNode
	{
		// Token: 0x0600AB84 RID: 43908 RVA: 0x00070462 File Offset: 0x0006E662
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB85 RID: 43909 RVA: 0x0007049A File Offset: 0x0006E69A
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

		// Token: 0x0600AB86 RID: 43910 RVA: 0x000704D0 File Offset: 0x0006E6D0
		private bool IsOfFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction == this.faction.GetValue(slate);
		}

		// Token: 0x040074E1 RID: 29921
		public SlateRef<Thing> thing;

		// Token: 0x040074E2 RID: 29922
		public SlateRef<Faction> faction;

		// Token: 0x040074E3 RID: 29923
		public QuestNode node;

		// Token: 0x040074E4 RID: 29924
		public QuestNode elseNode;
	}
}
