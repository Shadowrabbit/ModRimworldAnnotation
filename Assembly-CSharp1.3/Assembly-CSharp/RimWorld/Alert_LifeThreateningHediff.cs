using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001242 RID: 4674
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06007035 RID: 28725 RVA: 0x00255EF0 File Offset: 0x002540F0
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

		// Token: 0x06007036 RID: 28726 RVA: 0x00255FB0 File Offset: 0x002541B0
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x06007037 RID: 28727 RVA: 0x00255FC4 File Offset: 0x002541C4
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

		// Token: 0x06007038 RID: 28728 RVA: 0x002560F0 File Offset: 0x002542F0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}

		// Token: 0x04003DF6 RID: 15862
		private List<Pawn> sickPawnsResult = new List<Pawn>();
	}
}
