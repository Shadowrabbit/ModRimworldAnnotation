using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115C RID: 4444
	public class FocusStrengthOffset_BuildingDefsWithQuality : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x06006ACE RID: 27342 RVA: 0x0023DDAC File Offset: 0x0023BFAC
		protected override float OffsetForBuilding(Thing b)
		{
			QualityCategory qualityCategory;
			if (b.TryGetQuality(out qualityCategory))
			{
				return this.focusPerQuality.Evaluate((float)qualityCategory);
			}
			return 0f;
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x0023DDD8 File Offset: 0x0023BFD8
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

		// Token: 0x06006AD0 RID: 27344 RVA: 0x0023DE84 File Offset: 0x0023C084
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			string value = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKeyAbstract.Translate(this.maxBuildings, value) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x0023DF10 File Offset: 0x0023C110
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y;
		}

		// Token: 0x04003B64 RID: 15204
		public SimpleCurve focusPerQuality;
	}
}
