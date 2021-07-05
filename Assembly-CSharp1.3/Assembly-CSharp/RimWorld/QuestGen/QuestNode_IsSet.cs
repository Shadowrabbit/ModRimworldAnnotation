using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200162C RID: 5676
	public class QuestNode_IsSet : QuestNode
	{
		// Token: 0x060084E8 RID: 34024 RVA: 0x002FBEEC File Offset: 0x002FA0EC
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.name.GetValue(slate), false))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060084E9 RID: 34025 RVA: 0x002FBF3C File Offset: 0x002FA13C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.name.GetValue(slate), false))
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

		// Token: 0x0400529F RID: 21151
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040052A0 RID: 21152
		public QuestNode node;

		// Token: 0x040052A1 RID: 21153
		public QuestNode elseNode;
	}
}
