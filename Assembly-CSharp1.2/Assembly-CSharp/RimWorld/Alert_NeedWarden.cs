using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001947 RID: 6471
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x06008F5F RID: 36703 RVA: 0x00060059 File Offset: 0x0005E259
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F60 RID: 36704 RVA: 0x002944B8 File Offset: 0x002926B8
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
