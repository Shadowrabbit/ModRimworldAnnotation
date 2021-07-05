using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124B RID: 4683
	public class Alert_IdeoBuildingDisrespected : Alert_Precept
	{
		// Token: 0x06007058 RID: 28760 RVA: 0x00256EBA File Offset: 0x002550BA
		public Alert_IdeoBuildingDisrespected()
		{
		}

		// Token: 0x06007059 RID: 28761 RVA: 0x00256ED8 File Offset: 0x002550D8
		public Alert_IdeoBuildingDisrespected(IdeoBuildingPresenceDemand demand)
		{
			this.demand = demand;
			this.label = "IdeoBuildingDisrespected".Translate(demand.parent.LabelCap);
			this.sourcePrecept = demand.parent;
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x0600705A RID: 28762 RVA: 0x00256F3C File Offset: 0x0025513C
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.targetNames.Clear();
				Map currentMap = Find.CurrentMap;
				if (currentMap == null)
				{
					return null;
				}
				foreach (Pawn pawn in currentMap.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.Ideo == this.demand.parent.ideo)
					{
						this.targetNames.Add(pawn.LabelShort);
					}
				}
				Thing thing = this.demand.BestBuilding(Find.CurrentMap, false);
				if (thing != null)
				{
					this.targets.Add(thing);
				}
				return this.targets;
			}
		}

		// Token: 0x0600705B RID: 28763 RVA: 0x00257004 File Offset: 0x00255204
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			if (Find.CurrentMap == null || !this.demand.BuildingPresent(Find.CurrentMap) || this.demand.RequirementsSatisfied(Find.CurrentMap))
			{
				return false;
			}
			return new AlertReport
			{
				active = true,
				culpritsTargets = this.Targets
			};
		}

		// Token: 0x0600705C RID: 28764 RVA: 0x00257070 File Offset: 0x00255270
		public override TaggedString GetExplanation()
		{
			return "IdeoBuildingDisrespectedDesc".Translate(this.demand.parent.ideo.name, this.demand.parent.Label.Colorize(ColoredText.NameColor), this.demand.RoomRequirementsInfo.ToLineList(" -  "), this.targetNames.ToLineList(" -  "));
		}

		// Token: 0x04003E07 RID: 15879
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x04003E08 RID: 15880
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E09 RID: 15881
		private List<string> targetNames = new List<string>();
	}
}
