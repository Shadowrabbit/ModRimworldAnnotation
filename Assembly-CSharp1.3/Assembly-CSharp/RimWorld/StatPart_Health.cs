using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D2 RID: 5330
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x06007F21 RID: 32545 RVA: 0x002CF816 File Offset: 0x002CDA16
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints && req.Thing.def.healthAffectsPrice;
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x002CF847 File Offset: 0x002CDA47
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x06007F23 RID: 32547 RVA: 0x002CF864 File Offset: 0x002CDA64
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints);
		}
	}
}
