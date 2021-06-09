using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E7 RID: 6119
	public class FocusStrengthOffset_Quality : FocusStrengthOffset_Curve
	{
		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x06008777 RID: 34679 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool NeedsToBeSpawned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008778 RID: 34680 RVA: 0x0027BC84 File Offset: 0x00279E84
		protected override float SourceValue(Thing parent)
		{
			QualityCategory qualityCategory;
			parent.TryGetQuality(out qualityCategory);
			return (float)qualityCategory;
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x0005AEC3 File Offset: 0x000590C3
		public override float MaxOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return 0f;
			}
			return base.MaxOffset(null);
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x0027BC9C File Offset: 0x00279E9C
		public override float MinOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return this.GetOffset(parent, null);
			}
			return this.curve[0].y;
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x0600877B RID: 34683 RVA: 0x0005AED5 File Offset: 0x000590D5
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_FromQuality";
			}
		}
	}
}
