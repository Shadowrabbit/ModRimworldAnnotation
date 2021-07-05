using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5E RID: 3934
	public class RitualOutcomeComp_BuildingOfDefPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06005D5A RID: 23898 RVA: 0x00200340 File Offset: 0x001FE540
		protected override string LabelForDesc
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x06005D5B RID: 23899 RVA: 0x00200352 File Offset: 0x001FE552
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			return cell.GetFirstThing(map, this.def);
		}

		// Token: 0x040035FD RID: 13821
		public ThingDef def;
	}
}
