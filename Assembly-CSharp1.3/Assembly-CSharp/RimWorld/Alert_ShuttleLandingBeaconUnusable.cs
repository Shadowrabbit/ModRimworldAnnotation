using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127E RID: 4734
	public class Alert_ShuttleLandingBeaconUnusable : Alert
	{
		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06007127 RID: 28967 RVA: 0x0025B454 File Offset: 0x00259654
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

		// Token: 0x06007128 RID: 28968 RVA: 0x0025B4F1 File Offset: 0x002596F1
		public Alert_ShuttleLandingBeaconUnusable()
		{
			this.defaultLabel = "ShipLandingBeaconUnusable".Translate();
			this.defaultExplanation = "ShipLandingBeaconUnusableDesc".Translate();
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x0025B52E File Offset: 0x0025972E
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E3D RID: 15933
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
