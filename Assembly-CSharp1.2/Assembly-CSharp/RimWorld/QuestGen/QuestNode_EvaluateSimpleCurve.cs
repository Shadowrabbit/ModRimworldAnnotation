using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F0F RID: 7951
	public class QuestNode_EvaluateSimpleCurve : QuestNode
	{
		// Token: 0x0600AA2E RID: 43566 RVA: 0x0006F8F6 File Offset: 0x0006DAF6
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA2F RID: 43567 RVA: 0x0006F900 File Offset: 0x0006DB00
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA30 RID: 43568 RVA: 0x0031B070 File Offset: 0x00319270
		private void SetVars(Slate slate)
		{
			float num = this.curve.GetValue(slate).Evaluate(this.value.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04007395 RID: 29589
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007396 RID: 29590
		public SlateRef<SimpleCurve> curve;

		// Token: 0x04007397 RID: 29591
		public SlateRef<float> value;

		// Token: 0x04007398 RID: 29592
		public SlateRef<bool> roundRandom;
	}
}
