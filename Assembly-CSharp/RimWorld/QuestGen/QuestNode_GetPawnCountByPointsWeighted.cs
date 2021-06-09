using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F3D RID: 7997
	public class QuestNode_GetPawnCountByPointsWeighted : QuestNode
	{
		// Token: 0x0600AAD4 RID: 43732 RVA: 0x0006FDA3 File Offset: 0x0006DFA3
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AAD5 RID: 43733 RVA: 0x0006FDAD File Offset: 0x0006DFAD
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AAD6 RID: 43734 RVA: 0x0031D7A8 File Offset: 0x0031B9A8
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

		// Token: 0x0400742D RID: 29741
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400742E RID: 29742
		public SlateRef<int> challengeRating;

		// Token: 0x0400742F RID: 29743
		public SlateRef<int> maxCountOneStar;

		// Token: 0x04007430 RID: 29744
		public SlateRef<int> maxCountTwoStar;

		// Token: 0x04007431 RID: 29745
		public SlateRef<int> maxCountThreeStar;

		// Token: 0x04007432 RID: 29746
		public SlateRef<SimpleCurve> pointsCurve;

		// Token: 0x04007433 RID: 29747
		public SlateRef<SimpleCurve> chancesCurve;

		// Token: 0x04007434 RID: 29748
		public SlateRef<bool> roundRandom;
	}
}
