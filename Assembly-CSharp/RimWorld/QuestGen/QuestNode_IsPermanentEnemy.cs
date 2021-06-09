using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F72 RID: 8050
	public class QuestNode_IsPermanentEnemy : QuestNode
	{
		// Token: 0x0600AB8C RID: 43916 RVA: 0x000705AF File Offset: 0x0006E7AF
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsPermanentEnemy(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB8D RID: 43917 RVA: 0x000705E7 File Offset: 0x0006E7E7
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

		// Token: 0x0600AB8E RID: 43918 RVA: 0x0031F880 File Offset: 0x0031DA80
		private bool IsPermanentEnemy(Slate slate)
		{
			Thing value = this.thing.GetValue(slate);
			return value != null && value.Faction != null && value.Faction.def.permanentEnemy;
		}

		// Token: 0x040074E8 RID: 29928
		public SlateRef<Thing> thing;

		// Token: 0x040074E9 RID: 29929
		public QuestNode node;

		// Token: 0x040074EA RID: 29930
		public QuestNode elseNode;
	}
}
