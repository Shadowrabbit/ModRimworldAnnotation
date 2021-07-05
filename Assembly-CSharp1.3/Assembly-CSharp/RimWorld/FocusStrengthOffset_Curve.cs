using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001152 RID: 4434
	public abstract class FocusStrengthOffset_Curve : FocusStrengthOffset
	{
		// Token: 0x06006A93 RID: 27283
		protected abstract float SourceValue(Thing parent);

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06006A94 RID: 27284
		protected abstract string ExplanationKey { get; }

		// Token: 0x06006A95 RID: 27285 RVA: 0x0023D333 File Offset: 0x0023B533
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return Mathf.Round(this.curve.Evaluate(this.SourceValue(parent)) * 100f) / 100f;
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x0023D358 File Offset: 0x0023B558
		public override string GetExplanation(Thing parent)
		{
			return this.ExplanationKey.Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x0023D38C File Offset: 0x0023B58C
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.ExplanationKey.Translate() + ": " + (this.curve[0].y.ToStringWithSign("0%") + " " + "RangeTo".Translate() + " " + this.curve[this.curve.PointsCount - 1].y.ToStringWithSign("0%"));
		}

		// Token: 0x06006A98 RID: 27288 RVA: 0x0023D428 File Offset: 0x0023B628
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

		// Token: 0x04003B58 RID: 15192
		public SimpleCurve curve;
	}
}
