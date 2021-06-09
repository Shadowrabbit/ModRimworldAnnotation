using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA5 RID: 8101
	public class QuestNode_LoopCount : QuestNode
	{
		// Token: 0x0600AC2F RID: 44079 RVA: 0x003214CC File Offset: 0x0031F6CC
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

		// Token: 0x0600AC30 RID: 44080 RVA: 0x00321544 File Offset: 0x0031F744
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

		// Token: 0x0400759A RID: 30106
		public QuestNode node;

		// Token: 0x0400759B RID: 30107
		public SlateRef<int> loopCount;

		// Token: 0x0400759C RID: 30108
		[NoTranslate]
		public SlateRef<string> storeLoopCounterAs;
	}
}
