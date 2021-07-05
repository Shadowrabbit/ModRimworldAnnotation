using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200169F RID: 5791
	public class QuestNode_IsPermanentEnemy : QuestNode
	{
		// Token: 0x0600868C RID: 34444 RVA: 0x00303DE2 File Offset: 0x00301FE2
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsPermanentEnemy(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600868D RID: 34445 RVA: 0x00303E1A File Offset: 0x0030201A
		protected override void RunInt()
		{
			if (this.IsPermanentEnemy(QuestGen.slate))
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

		// Token: 0x0600868E RID: 34446 RVA: 0x00303E50 File Offset: 0x00302050
		private bool IsPermanentEnemy(Slate slate)
		{
			Thing value = this.thing.GetValue(slate);
			return value != null && value.Faction != null && value.Faction.def.permanentEnemy;
		}

		// Token: 0x04005465 RID: 21605
		public SlateRef<Thing> thing;

		// Token: 0x04005466 RID: 21606
		public QuestNode node;

		// Token: 0x04005467 RID: 21607
		public QuestNode elseNode;
	}
}
