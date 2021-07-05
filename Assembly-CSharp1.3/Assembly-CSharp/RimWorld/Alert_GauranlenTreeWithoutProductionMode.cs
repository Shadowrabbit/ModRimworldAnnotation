using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128C RID: 4748
	public class Alert_GauranlenTreeWithoutProductionMode : Alert
	{
		// Token: 0x06007166 RID: 29030 RVA: 0x0025CF7D File Offset: 0x0025B17D
		public Alert_GauranlenTreeWithoutProductionMode()
		{
			this.defaultLabel = "AlertGauranlenTreeWithoutDryadTypeLabel".Translate();
		}

		// Token: 0x06007167 RID: 29031 RVA: 0x0025CFA5 File Offset: 0x0025B1A5
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x06007168 RID: 29032 RVA: 0x0025CFC8 File Offset: 0x0025B1C8
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.DryadSpawner);
					for (int j = 0; j < list.Count; j++)
					{
						CompTreeConnection compTreeConnection = list[j].TryGetComp<CompTreeConnection>();
						if (compTreeConnection != null && compTreeConnection.Connected && !compTreeConnection.HasProductionMode)
						{
							this.targets.Add(list[j]);
						}
					}
				}
			}
		}

		// Token: 0x06007169 RID: 29033 RVA: 0x0025D065 File Offset: 0x0025B265
		public override TaggedString GetExplanation()
		{
			return "AlertGauranlenTreeWithoutDryadTypeDesc".Translate("ChangeMode".Translate());
		}

		// Token: 0x04003E5E RID: 15966
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
