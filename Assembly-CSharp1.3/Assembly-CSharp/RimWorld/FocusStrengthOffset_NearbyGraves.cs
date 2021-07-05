using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115D RID: 4445
	public class FocusStrengthOffset_NearbyGraves : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x06006AD3 RID: 27347 RVA: 0x0023DF58 File Offset: 0x0023C158
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

		// Token: 0x06006AD4 RID: 27348 RVA: 0x0023DFA5 File Offset: 0x0023C1A5
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * (this.focusPerFullGrave + base.MaxOffsetPerBuilding);
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x0023DFBC File Offset: 0x0023C1BC
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x0023E060 File Offset: 0x0023C260
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x04003B65 RID: 15205
		public float focusPerFullGrave;
	}
}
