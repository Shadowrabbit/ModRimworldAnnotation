using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3A RID: 7482
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x0600A299 RID: 41625 RVA: 0x0006C043 File Offset: 0x0006A243
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints && req.Thing.def.healthAffectsPrice;
		}

		// Token: 0x0600A29A RID: 41626 RVA: 0x0006C074 File Offset: 0x0006A274
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x0600A29B RID: 41627 RVA: 0x002F58B4 File Offset: 0x002F3AB4
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints);
		}
	}
}
