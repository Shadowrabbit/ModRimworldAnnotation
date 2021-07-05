using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200167F RID: 5759
	public class QuestNode_GetPawnCountByPointsWeighted : QuestNode
	{
		// Token: 0x0600860B RID: 34315 RVA: 0x00301790 File Offset: 0x002FF990
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600860C RID: 34316 RVA: 0x0030179A File Offset: 0x002FF99A
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600860D RID: 34317 RVA: 0x003017A8 File Offset: 0x002FF9A8
		private void SetVars(Slate slate)
		{
			float x = slate.Get<float>("points", 0f, false);
			float num = this.pointsCurve.GetValue(slate).Evaluate(x);
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			int num2;
			if (this.challengeRating.TryGetValue(slate, out num2))
			{
				if (num2 == 1)
				{
					num = Mathf.Min(num, (float)this.maxCountOneStar.GetValue(slate));
				}
				else if (num2 == 2)
				{
					num = Mathf.Min(num, (float)this.maxCountTwoStar.GetValue(slate));
				}
				else
				{
					num = Mathf.Min(num, (float)this.maxCountThreeStar.GetValue(slate));
				}
			}
			SimpleCurve value = this.chancesCurve.GetValue(slate);
			for (int i = value.Points.Count - 1; i >= 0; i--)
			{
				if (value.Points[i].x <= num)
				{
					value.Points.Insert(i + 1, new CurvePoint(num + 1f, 0f));
					break;
				}
				value.Points[i] = new CurvePoint(0f, 0f);
			}
			float num3 = Rand.ByCurve(value);
			if (this.roundRandom.GetValue(slate))
			{
				num3 = (float)GenMath.RoundRandom(num3);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), Mathf.Clamp(num3, 1f, num), false);
		}

		// Token: 0x040053DD RID: 21469
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053DE RID: 21470
		public SlateRef<int> challengeRating;

		// Token: 0x040053DF RID: 21471
		public SlateRef<int> maxCountOneStar;

		// Token: 0x040053E0 RID: 21472
		public SlateRef<int> maxCountTwoStar;

		// Token: 0x040053E1 RID: 21473
		public SlateRef<int> maxCountThreeStar;

		// Token: 0x040053E2 RID: 21474
		public SlateRef<SimpleCurve> pointsCurve;

		// Token: 0x040053E3 RID: 21475
		public SlateRef<SimpleCurve> chancesCurve;

		// Token: 0x040053E4 RID: 21476
		public SlateRef<bool> roundRandom;
	}
}
