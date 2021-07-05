using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200162B RID: 5675
	public class QuestNode_IsNull : QuestNode
	{
		// Token: 0x060084E5 RID: 34021 RVA: 0x002FBE66 File Offset: 0x002FA066
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == null)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084E6 RID: 34022 RVA: 0x002FBEA4 File Offset: 0x002FA0A4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == null)
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

		// Token: 0x0400529C RID: 21148
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x0400529D RID: 21149
		public QuestNode node;

		// Token: 0x0400529E RID: 21150
		public QuestNode elseNode;
	}
}
