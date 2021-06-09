using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F71 RID: 8049
	public class QuestNode_IsOfRoyalFaction : QuestNode
	{
		// Token: 0x0600AB88 RID: 43912 RVA: 0x00070501 File Offset: 0x0006E701
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfRoyalFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB89 RID: 43913 RVA: 0x00070539 File Offset: 0x0006E739
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

		// Token: 0x0600AB8A RID: 43914 RVA: 0x0007056F File Offset: 0x0006E76F
		private bool IsOfRoyalFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction != null && this.thing.GetValue(slate).Faction.def.HasRoyalTitles;
		}

		// Token: 0x040074E5 RID: 29925
		public SlateRef<Thing> thing;

		// Token: 0x040074E6 RID: 29926
		public QuestNode node;

		// Token: 0x040074E7 RID: 29927
		public QuestNode elseNode;
	}
}
