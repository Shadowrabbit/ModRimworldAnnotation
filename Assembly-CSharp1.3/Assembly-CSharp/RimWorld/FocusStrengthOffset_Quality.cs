using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001153 RID: 4435
	public class FocusStrengthOffset_Quality : FocusStrengthOffset_Curve
	{
		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06006A9A RID: 27290 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool NeedsToBeSpawned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x0023D480 File Offset: 0x0023B680
		protected override float SourceValue(Thing parent)
		{
			QualityCategory qualityCategory;
			parent.TryGetQuality(out qualityCategory);
			return (float)qualityCategory;
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x0023D498 File Offset: 0x0023B698
		public override float MaxOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return 0f;
			}
			return base.MaxOffset(null);
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x0023D4AC File Offset: 0x0023B6AC
		public override float MinOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return this.GetOffset(parent, null);
			}
			return this.curve[0].y;
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06006A9E RID: 27294 RVA: 0x0023D4D9 File Offset: 0x0023B6D9
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_FromQuality";
			}
		}
	}
}
