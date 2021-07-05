using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C21 RID: 3105
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		// Token: 0x060048E9 RID: 18665 RVA: 0x00181C0C File Offset: 0x0017FE0C
		protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, map, desperate)
			select f;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x00181C50 File Offset: 0x0017FE50
		protected virtual bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return !f.IsPlayer && !f.defeated && !f.temporary && (desperate || (f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp)));
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x00181CBC File Offset: 0x0017FEBC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return parms.faction != null || this.CandidateFactions(map, false).Any<Faction>();
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x00181CEC File Offset: 0x0017FEEC
		public string DebugListingOfGroupSources()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				stringBuilder.Append(faction.Name);
				if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, false))
				{
					stringBuilder.Append("    YES");
				}
				else if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, true))
				{
					stringBuilder.Append("    YES-DESPERATE");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
