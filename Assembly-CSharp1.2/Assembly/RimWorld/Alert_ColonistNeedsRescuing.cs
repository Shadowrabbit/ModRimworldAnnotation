using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001939 RID: 6457
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x06008F25 RID: 36645 RVA: 0x00293818 File Offset: 0x00291A18
		private List<Pawn> ColonistsNeedingRescue
		{
			get
			{
				this.colonistsNeedingRescueResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(pawn))
					{
						this.colonistsNeedingRescueResult.Add(pawn);
					}
				}
				return this.colonistsNeedingRescueResult;
			}
		}

		// Token: 0x06008F26 RID: 36646 RVA: 0x00293888 File Offset: 0x00291A88
		public static bool NeedsRescue(Pawn p)
		{
			if (p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker))
			{
				Pawn_JobTracker jobs = p.jobs;
				return ((jobs != null) ? jobs.jobQueue : null) == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false);
			}
			return false;
		}

		// Token: 0x06008F27 RID: 36647 RVA: 0x0005FD03 File Offset: 0x0005DF03
		public override string GetLabel()
		{
			if (this.ColonistsNeedingRescue.Count == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		// Token: 0x06008F28 RID: 36648 RVA: 0x002938FC File Offset: 0x00291AFC
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F29 RID: 36649 RVA: 0x0005FD32 File Offset: 0x0005DF32
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}

		// Token: 0x04005B52 RID: 23378
		private List<Pawn> colonistsNeedingRescueResult = new List<Pawn>();
	}
}
