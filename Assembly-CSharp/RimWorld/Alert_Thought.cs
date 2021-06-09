using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001973 RID: 6515
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06009009 RID: 36873
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x0600900A RID: 36874 RVA: 0x00297428 File Offset: 0x00295628
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

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x0600900B RID: 36875 RVA: 0x00297460 File Offset: 0x00295660
		private List<Pawn> AffectedPawns
		{
			get
			{
				this.affectedPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (pawn.Dead)
					{
						Log.Error("Dead pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists:" + pawn, false);
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

		// Token: 0x0600900C RID: 36876 RVA: 0x00060B13 File Offset: 0x0005ED13
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns);
		}

		// Token: 0x0600900D RID: 36877 RVA: 0x00297574 File Offset: 0x00295774
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}

		// Token: 0x04005B98 RID: 23448
		protected string explanationKey;

		// Token: 0x04005B99 RID: 23449
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x04005B9A RID: 23450
		private List<Pawn> affectedPawnsResult = new List<Pawn>();
	}
}
