using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E7 RID: 5351
	public class StatPart_Terror : StatPart
	{
		// Token: 0x06007F7A RID: 32634 RVA: 0x002D0C74 File Offset: 0x002CEE74
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn thing;
			if ((thing = (req.Thing as Pawn)) == null)
			{
				return;
			}
			val += TerrorUtility.SuppressionFallRateOverTerror.Evaluate(thing.GetStatValue(StatDefOf.Terror, true));
		}

		// Token: 0x06007F7B RID: 32635 RVA: 0x002D0CB0 File Offset: 0x002CEEB0
		public override string ExplanationPart(StatRequest req)
		{
			Pawn thing;
			if (req.HasThing && (thing = (req.Thing as Pawn)) != null && !Mathf.Approximately(TerrorUtility.SuppressionFallRateOverTerror.Evaluate(thing.GetStatValue(StatDefOf.Terror, true)), 0f))
			{
				return "StatsReport_Terror".Translate() + (": " + TerrorUtility.SuppressionFallRateOverTerror.Evaluate(thing.GetStatValue(StatDefOf.Terror, true)).ToStringPercent());
			}
			return null;
		}
	}
}
