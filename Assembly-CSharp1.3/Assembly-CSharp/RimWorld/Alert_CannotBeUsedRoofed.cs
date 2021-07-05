using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127B RID: 4731
	public class Alert_CannotBeUsedRoofed : Alert
	{
		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x0600711E RID: 28958 RVA: 0x0025B100 File Offset: 0x00259300
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

		// Token: 0x0600711F RID: 28959 RVA: 0x0025B230 File Offset: 0x00259430
		public Alert_CannotBeUsedRoofed()
		{
			this.defaultLabel = "BuildingCantBeUsedRoofed".Translate();
			this.defaultExplanation = "BuildingCantBeUsedRoofedDesc".Translate();
		}

		// Token: 0x06007120 RID: 28960 RVA: 0x0025B26D File Offset: 0x0025946D
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnusableBuildings);
		}

		// Token: 0x04003E39 RID: 15929
		private List<ThingDef> thingDefsToCheck;

		// Token: 0x04003E3A RID: 15930
		private List<Thing> unusableBuildingsResult = new List<Thing>();
	}
}
