using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E6 RID: 6118
	public abstract class FocusStrengthOffset_Curve : FocusStrengthOffset
	{
		// Token: 0x06008770 RID: 34672
		protected abstract float SourceValue(Thing parent);

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x06008771 RID: 34673
		protected abstract string ExplanationKey { get; }

		// Token: 0x06008772 RID: 34674 RVA: 0x0005AE63 File Offset: 0x00059063
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return Mathf.Round(this.curve.Evaluate(this.SourceValue(parent)) * 100f) / 100f;
		}

		// Token: 0x06008773 RID: 34675 RVA: 0x0005AE88 File Offset: 0x00059088
		public override string GetExplanation(Thing parent)
		{
			return this.ExplanationKey.Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x06008774 RID: 34676 RVA: 0x0027BB98 File Offset: 0x00279D98
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.ExplanationKey.Translate() + ": " + (this.curve[0].y.ToStringWithSign("0%") + " " + "RangeTo".Translate() + " " + this.curve[this.curve.PointsCount - 1].y.ToStringWithSign("0%"));
		}

		// Token: 0x06008775 RID: 34677 RVA: 0x0027BC34 File Offset: 0x00279E34
		public override float MaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.curve.PointsCount; i++)
			{
				float y = this.curve[i].y;
				if (Mathf.Abs(y) > Mathf.Abs(num))
				{
					num = y;
				}
			}
			return num;
		}

		// Token: 0x040056FE RID: 22270
		public SimpleCurve curve;
	}
}
