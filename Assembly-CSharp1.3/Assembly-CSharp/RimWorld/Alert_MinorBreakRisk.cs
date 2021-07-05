using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001252 RID: 4690
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06007077 RID: 28791 RVA: 0x002574EB File Offset: 0x002556EB
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06007078 RID: 28792 RVA: 0x00255B8D File Offset: 0x00253D8D
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06007079 RID: 28793 RVA: 0x00255B94 File Offset: 0x00253D94
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x0600707A RID: 28794 RVA: 0x002574FA File Offset: 0x002556FA
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
