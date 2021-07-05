using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001666 RID: 5734
	public class QuestNode_EvaluateSimpleCurve : QuestNode
	{
		// Token: 0x0600859C RID: 34204 RVA: 0x002FF011 File Offset: 0x002FD211
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600859D RID: 34205 RVA: 0x002FF01B File Offset: 0x002FD21B
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600859E RID: 34206 RVA: 0x002FF028 File Offset: 0x002FD228
		private void SetVars(Slate slate)
		{
			float num = this.curve.GetValue(slate).Evaluate(this.value.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04005374 RID: 21364
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005375 RID: 21365
		public SlateRef<SimpleCurve> curve;

		// Token: 0x04005376 RID: 21366
		public SlateRef<float> value;

		// Token: 0x04005377 RID: 21367
		public SlateRef<bool> roundRandom;
	}
}
