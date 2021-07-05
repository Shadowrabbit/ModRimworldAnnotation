using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123F RID: 4671
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06007025 RID: 28709 RVA: 0x00255B5A File Offset: 0x00253D5A
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

		// Token: 0x06007026 RID: 28710 RVA: 0x00255B8D File Offset: 0x00253D8D
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x00255B94 File Offset: 0x00253D94
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x00255BA0 File Offset: 0x00253DA0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Culprits);
		}

		// Token: 0x04003DF3 RID: 15859
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
