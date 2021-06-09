using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001941 RID: 6465
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06008F47 RID: 36679 RVA: 0x0005FEDB File Offset: 0x0005E0DB
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F48 RID: 36680 RVA: 0x0005FC88 File Offset: 0x0005DE88
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06008F49 RID: 36681 RVA: 0x0005FC8F File Offset: 0x0005DE8F
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06008F4A RID: 36682 RVA: 0x0005FEEA File Offset: 0x0005E0EA
		public override AlertReport GetReport()
		{
			if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>() || BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
			{
				return false;
			}
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskMinor);
		}
	}
}
