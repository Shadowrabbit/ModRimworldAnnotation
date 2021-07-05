using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFC RID: 3836
	public class PreceptRequirement_Altar : PreceptRequirement
	{
		// Token: 0x06005B72 RID: 23410 RVA: 0x001F96DC File Offset: 0x001F78DC
		public override bool Met(List<Precept> precepts)
		{
			for (int i = 0; i < precepts.Count; i++)
			{
				Precept_Building precept_Building;
				if ((precept_Building = (precepts[i] as Precept_Building)) != null && precept_Building.ThingDef.isAltar)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x001F971C File Offset: 0x001F791C
		public override Precept MakePrecept(Ideo ideo)
		{
			PreceptDef ideoBuilding = PreceptDefOf.IdeoBuilding;
			Precept_Building precept_Building = (Precept_Building)PreceptMaker.MakePrecept(ideoBuilding);
			precept_Building.ThingDef = (from b in ideoBuilding.Worker.ThingDefsForIdeo(ideo)
			where b.def.isAltar
			select b).RandomElement<PreceptThingChance>().def;
			return precept_Building;
		}
	}
}
