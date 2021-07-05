using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FE RID: 5374
	public class StatWorker_ShootingAccuracy : StatWorker
	{
		// Token: 0x06008011 RID: 32785 RVA: 0x002D5A98 File Offset: 0x002D3C98
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
