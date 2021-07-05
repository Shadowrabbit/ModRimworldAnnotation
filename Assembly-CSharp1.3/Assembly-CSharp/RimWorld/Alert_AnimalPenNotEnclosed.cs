using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001283 RID: 4739
	public class Alert_AnimalPenNotEnclosed : Alert
	{
		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x0600713B RID: 28987 RVA: 0x0025BE3F File Offset: 0x0025A03F
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.CalculateTargets();
				return this.targets;
			}
		}

		// Token: 0x0600713C RID: 28988 RVA: 0x0025BE4D File Offset: 0x0025A04D
		public Alert_AnimalPenNotEnclosed()
		{
			this.defaultLabel = "AlertAnimalPenNotEnclosed".Translate();
		}

		// Token: 0x0600713D RID: 28989 RVA: 0x0025BE78 File Offset: 0x0025A078
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
					{
						CompAnimalPenMarker compAnimalPenMarker = building.TryGetComp<CompAnimalPenMarker>();
						if (!building.IsForbidden(Faction.OfPlayer) && compAnimalPenMarker.PenState.Unenclosed)
						{
							this.targets.Add(building);
						}
					}
				}
			}
		}

		// Token: 0x0600713E RID: 28990 RVA: 0x0025BF50 File Offset: 0x0025A150
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalPenNotEnclosedDesc".Translate();
		}

		// Token: 0x0600713F RID: 28991 RVA: 0x0025BF5C File Offset: 0x0025A15C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E49 RID: 15945
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
