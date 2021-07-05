using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128B RID: 4747
	public class Alert_AgeReversalDemandNear : Alert
	{
		// Token: 0x06007162 RID: 29026 RVA: 0x0025CDA6 File Offset: 0x0025AFA6
		public Alert_AgeReversalDemandNear()
		{
			this.defaultLabel = "AlertAgeReversalDemandNear".Translate();
		}

		// Token: 0x06007163 RID: 29027 RVA: 0x0025CDD0 File Offset: 0x0025AFD0
		private void CalcPawnsNearDeadline()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
			{
				Ideo ideo = pawn.Ideo;
				if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded) && ThoughtWorker_AgeReversalDemanded.CanHaveThought(pawn) && (float)pawn.ageTracker.AgeReversalDemandedDeadlineTicks <= 300000f)
				{
					this.targets.Add(pawn);
				}
			}
		}

		// Token: 0x06007164 RID: 29028 RVA: 0x0025CE64 File Offset: 0x0025B064
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertAgeReversalDemandDesc".Translate();
			foreach (Pawn pawn in this.targets)
			{
				long num = pawn.ageTracker.AgeReversalDemandedDeadlineTicks;
				string key;
				if (num > 0L)
				{
					if (pawn.ageTracker.LastAgeReversalReason == Pawn_AgeTracker.AgeReversalReason.Recruited)
					{
						key = "AlertAgeReversalDemandDesc_Recruit";
					}
					else if (pawn.ageTracker.LastAgeReversalReason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
					{
						key = "AlertAgeReversalDemandDesc_Next";
					}
					else
					{
						key = "AlertAgeReversalDemandDesc_Initial";
					}
				}
				else
				{
					num = -num;
					key = "AlertAgeReversalDemandDesc_Overdue";
				}
				taggedString += "\n -  " + key.Translate(pawn.Named("PAWN"), pawn.Faction.Named("FACTION"), ((int)num).ToStringTicksToPeriod(true, false, true, true).Named("DURATION"));
			}
			return taggedString;
		}

		// Token: 0x06007165 RID: 29029 RVA: 0x0025CF5C File Offset: 0x0025B15C
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			this.CalcPawnsNearDeadline();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04003E5C RID: 15964
		private const float WarnWhenCloserThanTicks = 300000f;

		// Token: 0x04003E5D RID: 15965
		private List<Pawn> targets = new List<Pawn>();
	}
}
