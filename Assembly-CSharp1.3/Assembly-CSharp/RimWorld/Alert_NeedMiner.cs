using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001257 RID: 4695
	public class Alert_NeedMiner : Alert
	{
		// Token: 0x0600708D RID: 28813 RVA: 0x00257B1F File Offset: 0x00255D1F
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600708E RID: 28814 RVA: 0x00257B58 File Offset: 0x00255D58
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
