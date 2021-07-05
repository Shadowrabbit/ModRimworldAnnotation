using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001258 RID: 4696
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x0600708F RID: 28815 RVA: 0x00257C74 File Offset: 0x00255E74
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06007090 RID: 28816 RVA: 0x00257CB0 File Offset: 0x00255EB0
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.PrisonersOfColonySpawned.Any<Pawn>())
				{
					bool flag = false;
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Warden) > 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return AlertReport.CulpritIs(map.mapPawns.PrisonersOfColonySpawned[0]);
					}
				}
			}
			return false;
		}
	}
}
