using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F48 RID: 8008
	public class QuestNode_GetRandomByCurve : QuestNode
	{
		// Token: 0x0600AAFB RID: 43771 RVA: 0x0006FE91 File Offset: 0x0006E091
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AAFC RID: 43772 RVA: 0x0006FE9B File Offset: 0x0006E09B
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AAFD RID: 43773 RVA: 0x0031DE28 File Offset: 0x0031C028
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

		// Token: 0x04007456 RID: 29782
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007457 RID: 29783
		public SlateRef<SimpleCurve> curve;

		// Token: 0x04007458 RID: 29784
		public SlateRef<bool> roundRandom;

		// Token: 0x04007459 RID: 29785
		public SlateRef<float?> min;

		// Token: 0x0400745A RID: 29786
		public SlateRef<float?> max;
	}
}
