using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001245 RID: 4677
	public class Alert_DisallowedBuildingInsideMonument : Alert_Critical
	{
		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x0600703F RID: 28735 RVA: 0x00256284 File Offset: 0x00254484
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

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06007040 RID: 28736 RVA: 0x00256314 File Offset: 0x00254514
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

		// Token: 0x06007041 RID: 28737 RVA: 0x002563A0 File Offset: 0x002545A0
		public Alert_DisallowedBuildingInsideMonument()
		{
			this.defaultLabel = "DisallowedBuildingInsideMonument".Translate();
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x002563C8 File Offset: 0x002545C8
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.DisallowedBuildings);
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x002563E3 File Offset: 0x002545E3
		public override TaggedString GetExplanation()
		{
			return "DisallowedBuildingInsideMonumentDesc".Translate(this.MinTicksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x04003DF8 RID: 15864
		private List<Thing> disallowedBuildingsResult = new List<Thing>();
	}
}
