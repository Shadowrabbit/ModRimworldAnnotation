using System;
using System.Collections.Generic;
using System.Linq;

namespace RimWorld.QuestGen
{
	// Token: 0x02001698 RID: 5784
	public class QuestNode_AnyHiddenSitePart : QuestNode
	{
		// Token: 0x06008670 RID: 34416 RVA: 0x0030390F File Offset: 0x00301B0F
		protected override bool TestRunInt(Slate slate)
		{
			if (this.AnyHiddenSitePart(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06008671 RID: 34417 RVA: 0x00303947 File Offset: 0x00301B47
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

		// Token: 0x06008672 RID: 34418 RVA: 0x00303980 File Offset: 0x00301B80
		private bool AnyHiddenSitePart(Slate slate)
		{
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				return value.Any((SitePartDef x) => x.defaultHidden);
			}
			return false;
		}

		// Token: 0x0400544E RID: 21582
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x0400544F RID: 21583
		public QuestNode node;

		// Token: 0x04005450 RID: 21584
		public QuestNode elseNode;
	}
}
