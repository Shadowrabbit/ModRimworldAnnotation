using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001241 RID: 4673
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x0600702F RID: 28719 RVA: 0x00255D34 File Offset: 0x00253F34
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

		// Token: 0x06007030 RID: 28720 RVA: 0x00255DA4 File Offset: 0x00253FA4
		public static bool NeedsRescue(Pawn p)
		{
			if (p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker))
			{
				Pawn_JobTracker jobs = p.jobs;
				return ((jobs != null) ? jobs.jobQueue : null) == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false);
			}
			return false;
		}

		// Token: 0x06007031 RID: 28721 RVA: 0x00255E18 File Offset: 0x00254018
		public override string GetLabel()
		{
			if (this.ColonistsNeedingRescue.Count == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x00255E48 File Offset: 0x00254048
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06007033 RID: 28723 RVA: 0x00255ED0 File Offset: 0x002540D0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}

		// Token: 0x04003DF5 RID: 15861
		private List<Pawn> colonistsNeedingRescueResult = new List<Pawn>();
	}
}
