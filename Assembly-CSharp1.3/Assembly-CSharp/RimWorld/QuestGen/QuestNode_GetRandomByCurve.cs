using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001685 RID: 5765
	public class QuestNode_GetRandomByCurve : QuestNode
	{
		// Token: 0x06008622 RID: 34338 RVA: 0x00301E63 File Offset: 0x00300063
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06008623 RID: 34339 RVA: 0x00301E6D File Offset: 0x0030006D
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008624 RID: 34340 RVA: 0x00301E7C File Offset: 0x0030007C
		private void SetVars(Slate slate)
		{
			float num = Rand.ByCurve(this.curve.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			if (this.min.GetValue(slate) != null)
			{
				num = Mathf.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Mathf.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x040053F5 RID: 21493
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053F6 RID: 21494
		public SlateRef<SimpleCurve> curve;

		// Token: 0x040053F7 RID: 21495
		public SlateRef<bool> roundRandom;

		// Token: 0x040053F8 RID: 21496
		public SlateRef<float?> min;

		// Token: 0x040053F9 RID: 21497
		public SlateRef<float?> max;
	}
}
