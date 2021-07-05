using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128D RID: 4749
	public class Alert_ConnectedPawnNotAssignedToPlantCutting : Alert
	{
		// Token: 0x0600716A RID: 29034 RVA: 0x0025D080 File Offset: 0x0025B280
		public Alert_ConnectedPawnNotAssignedToPlantCutting()
		{
			this.defaultLabel = "AlertConnectedPawnNotAssignedToPlantCuttingLabel".Translate();
		}

		// Token: 0x0600716B RID: 29035 RVA: 0x0025D0A8 File Offset: 0x0025B2A8
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600716C RID: 29036 RVA: 0x0025D0CC File Offset: 0x0025B2CC
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
						if (compTreeConnection != null && compTreeConnection.Connected && compTreeConnection.DesiredConnectionStrength <= 0f && compTreeConnection.ConnectedPawn.workSettings.GetPriority(WorkTypeDefOf.PlantCutting) == 0)
						{
							this.targets.Add(compTreeConnection.ConnectedPawn);
						}
					}
				}
			}
		}

		// Token: 0x0600716D RID: 29037 RVA: 0x0025D18C File Offset: 0x0025B38C
		public override TaggedString GetExplanation()
		{
			return "AlertConnectedPawnNotAssignedToPlantCuttingDesc".Translate() + ":\n" + (from x in this.targets
			select ((Pawn)((Thing)x)).NameFullColored.Resolve()).ToLineList("  - ", true);
		}

		// Token: 0x04003E5F RID: 15967
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
