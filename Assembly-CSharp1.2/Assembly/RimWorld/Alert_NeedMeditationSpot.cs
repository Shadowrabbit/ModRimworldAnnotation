using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001970 RID: 6512
	public class Alert_NeedMeditationSpot : Alert
	{
		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x06008FFB RID: 36859 RVA: 0x002970D8 File Offset: 0x002952D8
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnNames.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					bool flag = false;
					for (int i = 0; i < pawn.timetable.times.Count; i++)
					{
						if (pawn.timetable.times[i] == TimeAssignmentDefOf.Meditate)
						{
							flag = true;
							break;
						}
					}
					if ((pawn.HasPsylink || flag) && !MeditationUtility.AllMeditationSpotCandidates(pawn, false).Any<LocalTargetInfo>())
					{
						this.targets.Add(pawn);
						this.pawnNames.Add(pawn.LabelShort);
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06008FFC RID: 36860 RVA: 0x000609BE File Offset: 0x0005EBBE
		public Alert_NeedMeditationSpot()
		{
			this.defaultLabel = "NeedMeditationSpotAlert".Translate();
		}

		// Token: 0x06008FFD RID: 36861 RVA: 0x000609F1 File Offset: 0x0005EBF1
		public override TaggedString GetExplanation()
		{
			return "NeedMeditationSpotAlertDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x06008FFE RID: 36862 RVA: 0x00060A12 File Offset: 0x0005EC12
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04005B93 RID: 23443
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04005B94 RID: 23444
		private List<string> pawnNames = new List<string>();
	}
}
