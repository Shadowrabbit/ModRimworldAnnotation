using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127F RID: 4735
	public class Alert_NeedMeditationSpot : Alert
	{
		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x0600712A RID: 28970 RVA: 0x0025B54C File Offset: 0x0025974C
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

		// Token: 0x0600712B RID: 28971 RVA: 0x0025B62C File Offset: 0x0025982C
		public Alert_NeedMeditationSpot()
		{
			this.defaultLabel = "NeedMeditationSpotAlert".Translate();
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x0025B65F File Offset: 0x0025985F
		public override TaggedString GetExplanation()
		{
			return "NeedMeditationSpotAlertDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x0600712D RID: 28973 RVA: 0x0025B680 File Offset: 0x00259880
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E3E RID: 15934
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E3F RID: 15935
		private List<string> pawnNames = new List<string>();
	}
}
