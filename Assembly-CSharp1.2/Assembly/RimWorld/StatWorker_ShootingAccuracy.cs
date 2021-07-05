using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6B RID: 7531
	public class StatWorker_ShootingAccuracy : StatWorker
	{
		// Token: 0x0600A3BB RID: 41915 RVA: 0x002FAED4 File Offset: 0x002F90D4
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 5; i <= 45; i += 5)
			{
				float f = ShotReport.HitFactorFromShooter(finalVal, (float)i);
				stringBuilder.AppendLine("distance".Translate().CapitalizeFirst() + " " + i.ToString() + ": " + f.ToStringPercent("F1"));
			}
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			return stringBuilder.ToString();
		}
	}
}
