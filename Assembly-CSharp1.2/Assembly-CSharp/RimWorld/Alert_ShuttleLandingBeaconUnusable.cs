using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196F RID: 6511
	public class Alert_ShuttleLandingBeaconUnusable : Alert
	{
		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x06008FF8 RID: 36856 RVA: 0x00297038 File Offset: 0x00295238
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
					for (int j = 0; j < list.Count; j++)
					{
						CompShipLandingBeacon compShipLandingBeacon = list[j].TryGetComp<CompShipLandingBeacon>();
						if (compShipLandingBeacon != null && compShipLandingBeacon.Active && !compShipLandingBeacon.LandingAreas.Any<ShipLandingArea>())
						{
							this.targets.Add(list[j]);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06008FF9 RID: 36857 RVA: 0x00060966 File Offset: 0x0005EB66
		public Alert_ShuttleLandingBeaconUnusable()
		{
			this.defaultLabel = "ShipLandingBeaconUnusable".Translate();
			this.defaultExplanation = "ShipLandingBeaconUnusableDesc".Translate();
		}

		// Token: 0x06008FFA RID: 36858 RVA: 0x000609A3 File Offset: 0x0005EBA3
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04005B92 RID: 23442
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
