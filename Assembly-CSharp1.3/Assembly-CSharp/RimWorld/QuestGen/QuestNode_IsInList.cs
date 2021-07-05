using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200162A RID: 5674
	public class QuestNode_IsInList : QuestNode
	{
		// Token: 0x060084E2 RID: 34018 RVA: 0x002FBDB0 File Offset: 0x002F9FB0
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084E3 RID: 34019 RVA: 0x002FBE0C File Offset: 0x002FA00C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
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

		// Token: 0x04005298 RID: 21144
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04005299 RID: 21145
		public SlateRef<object> value;

		// Token: 0x0400529A RID: 21146
		public QuestNode node;

		// Token: 0x0400529B RID: 21147
		public QuestNode elseNode;
	}
}
