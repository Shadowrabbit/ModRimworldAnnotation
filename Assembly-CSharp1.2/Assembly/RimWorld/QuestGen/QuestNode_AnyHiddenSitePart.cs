using System;
using System.Collections.Generic;
using System.Linq;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F6A RID: 8042
	public class QuestNode_AnyHiddenSitePart : QuestNode
	{
		// Token: 0x0600AB6D RID: 43885 RVA: 0x000701FD File Offset: 0x0006E3FD
		protected override bool TestRunInt(Slate slate)
		{
			if (this.AnyHiddenSitePart(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600AB6E RID: 43886 RVA: 0x00070235 File Offset: 0x0006E435
		protected override void RunInt()
		{
			if (this.AnyHiddenSitePart(QuestGen.slate))
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

		// Token: 0x0600AB6F RID: 43887 RVA: 0x0031F754 File Offset: 0x0031D954
		private bool AnyHiddenSitePart(Slate slate)
		{
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				return value.Any((SitePartDef x) => x.defaultHidden);
			}
			return false;
		}

		// Token: 0x040074CF RID: 29903
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x040074D0 RID: 29904
		public QuestNode node;

		// Token: 0x040074D1 RID: 29905
		public QuestNode elseNode;
	}
}
