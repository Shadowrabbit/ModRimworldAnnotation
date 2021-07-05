using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200166E RID: 5742
	public class QuestNode_GetEventDelays : QuestNode
	{
		// Token: 0x060085BC RID: 34236 RVA: 0x002FF657 File Offset: 0x002FD857
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060085BD RID: 34237 RVA: 0x002FF661 File Offset: 0x002FD861
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085BE RID: 34238 RVA: 0x002FF670 File Offset: 0x002FD870
		private void SetVars(Slate slate)
		{
			if (this.intervalTicksRange.GetValue(slate).max <= 0)
			{
				Log.Error("intervalTicksRange with max <= 0");
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

		// Token: 0x0400538A RID: 21386
		public SlateRef<int> durationTicks;

		// Token: 0x0400538B RID: 21387
		public SlateRef<IntRange> intervalTicksRange;

		// Token: 0x0400538C RID: 21388
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x0400538D RID: 21389
		[NoTranslate]
		public SlateRef<string> storeDelaysAs;
	}
}
