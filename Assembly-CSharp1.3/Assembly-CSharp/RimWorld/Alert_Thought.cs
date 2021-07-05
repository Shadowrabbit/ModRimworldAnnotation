using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128E RID: 4750
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x0600716E RID: 29038
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x0600716F RID: 29039 RVA: 0x0025D1E8 File Offset: 0x0025B3E8
		public override string GetLabel()
		{
			int count = this.AffectedPawns.Count;
			string label = base.GetLabel();
			if (count > 1)
			{
				return string.Format("{0} x{1}", label, count.ToString());
			}
			return label;
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06007170 RID: 29040 RVA: 0x0025D220 File Offset: 0x0025B420
		private List<Pawn> AffectedPawns
		{
			get
			{
				this.affectedPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (pawn.Dead)
					{
						Log.Error("Dead pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists:" + pawn);
					}
					else if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
						try
						{
							ThoughtDef thought = this.Thought;
							for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
							{
								if (Alert_Thought.tmpThoughts[i].def == thought && !ThoughtUtility.ThoughtNullified(pawn, Alert_Thought.tmpThoughts[i].def))
								{
									this.affectedPawnsResult.Add(pawn);
								}
							}
						}
						finally
						{
							Alert_Thought.tmpThoughts.Clear();
						}
					}
				}
				return this.affectedPawnsResult;
			}
		}

		// Token: 0x06007171 RID: 29041 RVA: 0x0025D334 File Offset: 0x0025B534
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns);
		}

		// Token: 0x06007172 RID: 29042 RVA: 0x0025D344 File Offset: 0x0025B544
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}

		// Token: 0x04003E60 RID: 15968
		protected string explanationKey;

		// Token: 0x04003E61 RID: 15969
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x04003E62 RID: 15970
		private List<Pawn> affectedPawnsResult = new List<Pawn>();
	}
}
