using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001946 RID: 6470
	public class Alert_NeedMiner : Alert
	{
		// Token: 0x06008F5D RID: 36701 RVA: 0x00060020 File Offset: 0x0005E220
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F5E RID: 36702 RVA: 0x0029439C File Offset: 0x0029259C
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					Designation designation = null;
					List<Designation> allDesignations = map.designationManager.allDesignations;
					for (int j = 0; j < allDesignations.Count; j++)
					{
						if (allDesignations[j].def == DesignationDefOf.Mine)
						{
							designation = allDesignations[j];
							break;
						}
					}
					if (designation != null)
					{
						bool flag = false;
						foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Mining) > 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return AlertReport.CulpritIs(designation.target.Thing);
						}
					}
				}
			}
			return false;
		}
	}
}
