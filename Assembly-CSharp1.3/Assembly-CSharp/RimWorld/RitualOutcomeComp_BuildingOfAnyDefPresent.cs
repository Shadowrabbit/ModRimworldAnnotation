using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5F RID: 3935
	public class RitualOutcomeComp_BuildingOfAnyDefPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06005D5D RID: 23901 RVA: 0x0020036C File Offset: 0x001FE56C
		protected override string LabelForDesc
		{
			get
			{
				return "RitualOutcomeLabelAnyOfThese".Translate() + ": " + (from d in this.defs
				select d.LabelCap.Resolve()).ToCommaList(false, false);
			}
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x002003C8 File Offset: 0x001FE5C8
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			foreach (ThingDef def in this.defs)
			{
				Thing firstThing = cell.GetFirstThing(map, def);
				if (firstThing != null)
				{
					return firstThing;
				}
			}
			return null;
		}

		// Token: 0x040035FE RID: 13822
		public List<ThingDef> defs;
	}
}
