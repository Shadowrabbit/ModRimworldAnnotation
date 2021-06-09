using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196C RID: 6508
	public class Alert_CannotBeUsedRoofed : Alert
	{
		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06008FEF RID: 36847 RVA: 0x00296DD4 File Offset: 0x00294FD4
		private List<Thing> UnusableBuildings
		{
			get
			{
				this.unusableBuildingsResult.Clear();
				if (this.thingDefsToCheck == null)
				{
					this.thingDefsToCheck = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (!thingDef.canBeUsedUnderRoof)
						{
							this.thingDefsToCheck.Add(thingDef);
						}
					}
				}
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.thingDefsToCheck.Count; i++)
				{
					for (int j = 0; j < maps.Count; j++)
					{
						List<Thing> list = maps[j].listerThings.ThingsOfDef(this.thingDefsToCheck[i]);
						for (int k = 0; k < list.Count; k++)
						{
							if (list[k].Faction == ofPlayer && RoofUtility.IsAnyCellUnderRoof(list[k]))
							{
								this.unusableBuildingsResult.Add(list[k]);
							}
						}
					}
				}
				return this.unusableBuildingsResult;
			}
		}

		// Token: 0x06008FF0 RID: 36848 RVA: 0x0006087A File Offset: 0x0005EA7A
		public Alert_CannotBeUsedRoofed()
		{
			this.defaultLabel = "BuildingCantBeUsedRoofed".Translate();
			this.defaultExplanation = "BuildingCantBeUsedRoofedDesc".Translate();
		}

		// Token: 0x06008FF1 RID: 36849 RVA: 0x000608B7 File Offset: 0x0005EAB7
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnusableBuildings);
		}

		// Token: 0x04005B8E RID: 23438
		private List<ThingDef> thingDefsToCheck;

		// Token: 0x04005B8F RID: 23439
		private List<Thing> unusableBuildingsResult = new List<Thing>();
	}
}
