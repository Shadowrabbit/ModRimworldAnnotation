using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016CE RID: 5838
	public class QuestNode_LoopCount : QuestNode
	{
		// Token: 0x06008726 RID: 34598 RVA: 0x00306210 File Offset: 0x00304410
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					if (!this.node.TestRun(slate))
					{
						return false;
					}
				}
				finally
				{
					slate.PopPrefix();
				}
			}
			return true;
		}

		// Token: 0x06008727 RID: 34599 RVA: 0x00306288 File Offset: 0x00304488
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					QuestGen.slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					this.node.Run();
				}
				finally
				{
					QuestGen.slate.PopPrefix();
				}
			}
		}

		// Token: 0x0400552D RID: 21805
		public QuestNode node;

		// Token: 0x0400552E RID: 21806
		public SlateRef<int> loopCount;

		// Token: 0x0400552F RID: 21807
		[NoTranslate]
		public SlateRef<string> storeLoopCounterAs;
	}
}
