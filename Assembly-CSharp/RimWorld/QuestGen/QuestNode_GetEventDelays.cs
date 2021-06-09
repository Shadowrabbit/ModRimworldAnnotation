using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F1A RID: 7962
	public class QuestNode_GetEventDelays : QuestNode
	{
		// Token: 0x0600AA56 RID: 43606 RVA: 0x0006FA03 File Offset: 0x0006DC03
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA57 RID: 43607 RVA: 0x0006FA0D File Offset: 0x0006DC0D
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA58 RID: 43608 RVA: 0x0031B530 File Offset: 0x00319730
		private void SetVars(Slate slate)
		{
			if (this.intervalTicksRange.GetValue(slate).max <= 0)
			{
				Log.Error("intervalTicksRange with max <= 0", false);
				return;
			}
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num += this.intervalTicksRange.GetValue(slate).RandomInRange;
				if (num > this.durationTicks.GetValue(slate))
				{
					break;
				}
				slate.Set<int>(this.storeDelaysAs.GetValue(slate).Formatted(num2.Named("INDEX")), num, false);
				num2++;
			}
			slate.Set<int>(this.storeCountAs.GetValue(slate), num2, false);
		}

		// Token: 0x040073B2 RID: 29618
		public SlateRef<int> durationTicks;

		// Token: 0x040073B3 RID: 29619
		public SlateRef<IntRange> intervalTicksRange;

		// Token: 0x040073B4 RID: 29620
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x040073B5 RID: 29621
		[NoTranslate]
		public SlateRef<string> storeDelaysAs;
	}
}
