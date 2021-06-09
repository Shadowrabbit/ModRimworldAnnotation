using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F2 RID: 6130
	public class FocusStrengthOffset_BuildingDefsWithQuality : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x060087B0 RID: 34736 RVA: 0x0027C3B0 File Offset: 0x0027A5B0
		protected override float OffsetForBuilding(Thing b)
		{
			QualityCategory qualityCategory;
			if (b.TryGetQuality(out qualityCategory))
			{
				return this.focusPerQuality.Evaluate((float)qualityCategory);
			}
			return 0f;
		}

		// Token: 0x060087B1 RID: 34737 RVA: 0x0027C3DC File Offset: 0x0027A5DC
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			string value2 = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKey.Translate(value, this.maxBuildings, value2) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x060087B2 RID: 34738 RVA: 0x0027C488 File Offset: 0x0027A688
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			string value = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKeyAbstract.Translate(this.maxBuildings, value) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x060087B3 RID: 34739 RVA: 0x0027C514 File Offset: 0x0027A714
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y;
		}

		// Token: 0x0400570D RID: 22285
		public SimpleCurve focusPerQuality;
	}
}
