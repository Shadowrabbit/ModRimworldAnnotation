using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193D RID: 6461
	public class Alert_DisallowedBuildingInsideMonument : Alert_Critical
	{
		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x06008F35 RID: 36661 RVA: 0x00293C6C File Offset: 0x00291E6C
		private List<Thing> DisallowedBuildings
		{
			get
			{
				this.disallowedBuildingsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone)
						{
							Thing firstDisallowedBuilding = monumentMarker.FirstDisallowedBuilding;
							if (firstDisallowedBuilding != null)
							{
								this.disallowedBuildingsResult.Add(firstDisallowedBuilding);
							}
						}
					}
				}
				return this.disallowedBuildingsResult;
			}
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x06008F36 RID: 36662 RVA: 0x00293CFC File Offset: 0x00291EFC
		private int MinTicksLeft
		{
			get
			{
				int num = int.MaxValue;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone && monumentMarker.AnyDisallowedBuilding)
						{
							num = Mathf.Min(num, 60000 - monumentMarker.ticksSinceDisallowedBuilding);
						}
					}
				}
				return num;
			}
		}

		// Token: 0x06008F37 RID: 36663 RVA: 0x0005FDF9 File Offset: 0x0005DFF9
		public Alert_DisallowedBuildingInsideMonument()
		{
			this.defaultLabel = "DisallowedBuildingInsideMonument".Translate();
		}

		// Token: 0x06008F38 RID: 36664 RVA: 0x0005FE21 File Offset: 0x0005E021
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.DisallowedBuildings);
		}

		// Token: 0x06008F39 RID: 36665 RVA: 0x0005FE3C File Offset: 0x0005E03C
		public override TaggedString GetExplanation()
		{
			return "DisallowedBuildingInsideMonumentDesc".Translate(this.MinTicksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x04005B55 RID: 23381
		private List<Thing> disallowedBuildingsResult = new List<Thing>();
	}
}
