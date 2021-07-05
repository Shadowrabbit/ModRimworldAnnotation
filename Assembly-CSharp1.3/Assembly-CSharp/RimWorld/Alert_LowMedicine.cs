using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125A RID: 4698
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x06007095 RID: 28821 RVA: 0x00257ED1 File Offset: 0x002560D1
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06007096 RID: 28822 RVA: 0x00257EF8 File Offset: 0x002560F8
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

		// Token: 0x06007097 RID: 28823 RVA: 0x00257F40 File Offset: 0x00256140
		public override AlertReport GetReport()
		{
			if (Find.TickManager.TicksGame < 150000)
			{
				return false;
			}
			return this.MapWithLowMedicine() != null;
		}

		// Token: 0x06007098 RID: 28824 RVA: 0x00257F68 File Offset: 0x00256168
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

		// Token: 0x06007099 RID: 28825 RVA: 0x00257FC8 File Offset: 0x002561C8
		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}

		// Token: 0x04003E14 RID: 15892
		private const float MedicinePerColonistThreshold = 2f;
	}
}
