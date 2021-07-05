using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124A RID: 4682
	public class Alert_IdeoBuildingMissing : Alert_Precept
	{
		// Token: 0x06007053 RID: 28755 RVA: 0x00256C14 File Offset: 0x00254E14
		public Alert_IdeoBuildingMissing()
		{
		}

		// Token: 0x06007054 RID: 28756 RVA: 0x00256C34 File Offset: 0x00254E34
		public Alert_IdeoBuildingMissing(IdeoBuildingPresenceDemand demand)
		{
			this.demand = demand;
			this.label = "IdeoBuildingMissing".Translate(demand.parent.LabelCap);
			this.sourcePrecept = demand.parent;
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06007055 RID: 28757 RVA: 0x00256C98 File Offset: 0x00254E98
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
					if (!pawn.IsSlave && pawn.Ideo == this.demand.parent.ideo)
					{
						this.targets.Add(pawn);
						this.targetNames.Add(pawn.LabelShort);
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06007056 RID: 28758 RVA: 0x00256D54 File Offset: 0x00254F54
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || this.demand.BuildingPresent(currentMap))
			{
				return false;
			}
			return new AlertReport
			{
				active = true,
				culpritsTargets = this.Targets
			};
		}

		// Token: 0x06007057 RID: 28759 RVA: 0x00256DAC File Offset: 0x00254FAC
		public override TaggedString GetExplanation()
		{
			string value = "";
			if (!this.demand.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				value = "\n\n" + "IdeoBuildingRoomRequirementsDesc".Translate(this.demand.parent.Label) + ":\n" + this.demand.RoomRequirementsInfo.ToLineList(" -  ");
			}
			return "IdeoBuildingMissingDesc".Translate(this.demand.parent.Label.Colorize(ColoredText.NameColor), this.demand.parent.ideo.name, ("IdeoBuildingExpectations".Translate() + " " + this.demand.minExpectation.label).Colorize(ColoredText.ExpectationsColor), this.targetNames.ToLineList(" -  "), value);
		}

		// Token: 0x04003E04 RID: 15876
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x04003E05 RID: 15877
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E06 RID: 15878
		private List<string> targetNames = new List<string>();
	}
}
