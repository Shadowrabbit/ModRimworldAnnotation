using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001937 RID: 6455
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x06008F1B RID: 36635 RVA: 0x0005FC55 File Offset: 0x0005DE55
		private List<Pawn> Culprits
		{
			get
			{
				this.culpritsResult.Clear();
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskExtreme);
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskMajor);
				return this.culpritsResult;
			}
		}

		// Token: 0x06008F1C RID: 36636 RVA: 0x0005FC88 File Offset: 0x0005DE88
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06008F1D RID: 36637 RVA: 0x0005FC8F File Offset: 0x0005DE8F
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06008F1E RID: 36638 RVA: 0x0005FC9B File Offset: 0x0005DE9B
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Culprits);
		}

		// Token: 0x04005B50 RID: 23376
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
