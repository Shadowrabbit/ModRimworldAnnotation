using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F3 RID: 6131
	public class FocusStrengthOffset_NearbyGraves : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x060087B5 RID: 34741 RVA: 0x0027C554 File Offset: 0x0027A754
		protected override float OffsetForBuilding(Thing b)
		{
			float num = base.OffsetFor(b.def);
			Building_Grave building_Grave;
			if ((building_Grave = (b as Building_Grave)) != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike)
			{
				num += this.focusPerFullGrave;
			}
			return num;
		}

		// Token: 0x060087B6 RID: 34742 RVA: 0x0005B0D1 File Offset: 0x000592D1
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * (this.focusPerFullGrave + base.MaxOffsetPerBuilding);
		}

		// Token: 0x060087B7 RID: 34743 RVA: 0x0027C5A4 File Offset: 0x0027A7A4
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x060087B8 RID: 34744 RVA: 0x0027C648 File Offset: 0x0027A848
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x0400570E RID: 22286
		public float focusPerFullGrave;
	}
}
