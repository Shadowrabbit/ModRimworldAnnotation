using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EE8 RID: 7912
	public class QuestNode_IsSet : QuestNode
	{
		// Token: 0x0600A9B1 RID: 43441 RVA: 0x0031909C File Offset: 0x0031729C
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.name.GetValue(slate), false))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600A9B2 RID: 43442 RVA: 0x003190EC File Offset: 0x003172EC
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

		// Token: 0x040072F6 RID: 29430
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040072F7 RID: 29431
		public QuestNode node;

		// Token: 0x040072F8 RID: 29432
		public QuestNode elseNode;
	}
}
