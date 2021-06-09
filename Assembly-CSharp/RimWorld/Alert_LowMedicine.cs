using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001949 RID: 6473
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x06008F65 RID: 36709 RVA: 0x000600DE File Offset: 0x0005E2DE
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F66 RID: 36710 RVA: 0x00294690 File Offset: 0x00292890
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithLowMedicine();
			if (map == null)
			{
				return "";
			}
			int num = this.MedicineCount(map);
			if (num == 0)
			{
				return "NoMedicineDesc".Translate();
			}
			return "LowMedicineDesc".Translate(num);
		}

		// Token: 0x06008F67 RID: 36711 RVA: 0x00060102 File Offset: 0x0005E302
		public override AlertReport GetReport()
		{
			if (Find.TickManager.TicksGame < 150000)
			{
				return false;
			}
			return this.MapWithLowMedicine() != null;
		}

		// Token: 0x06008F68 RID: 36712 RVA: 0x002946D8 File Offset: 0x002928D8
		private Map MapWithLowMedicine()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned && (float)this.MedicineCount(map) < 2f * (float)map.mapPawns.FreeColonistsSpawnedCount)
				{
					return map;
				}
			}
			return null;
		}

		// Token: 0x06008F69 RID: 36713 RVA: 0x0006012A File Offset: 0x0005E32A
		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}

		// Token: 0x04005B61 RID: 23393
		private const float MedicinePerColonistThreshold = 2f;
	}
}
