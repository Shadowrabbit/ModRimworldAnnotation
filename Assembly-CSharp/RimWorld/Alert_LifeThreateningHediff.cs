using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193A RID: 6458
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x06008F2B RID: 36651 RVA: 0x00293984 File Offset: 0x00291B84
		private List<Pawn> SickPawns
		{
			get
			{
				this.sickPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
					{
						Hediff hediff = pawn.health.hediffSet.hediffs[i];
						if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && !hediff.FullyImmune())
						{
							this.sickPawnsResult.Add(pawn);
							break;
						}
					}
				}
				return this.sickPawnsResult;
			}
		}

		// Token: 0x06008F2C RID: 36652 RVA: 0x0005FD52 File Offset: 0x0005DF52
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x06008F2D RID: 36653 RVA: 0x00293A44 File Offset: 0x00291C44
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn pawn in this.SickPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && hediff.Part != null && hediff.Part != pawn.RaceProps.body.corePart)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return "PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(stringBuilder.ToString());
			}
			return "PawnsWithLifeThreateningDiseaseDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x0005FD63 File Offset: 0x0005DF63
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}

		// Token: 0x04005B53 RID: 23379
		private List<Pawn> sickPawnsResult = new List<Pawn>();
	}
}
