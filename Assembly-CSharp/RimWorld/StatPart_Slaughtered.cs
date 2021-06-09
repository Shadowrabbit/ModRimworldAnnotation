using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4A RID: 7498
	public class StatPart_Slaughtered : StatPart
	{
		// Token: 0x0600A2E1 RID: 41697 RVA: 0x0006C29F File Offset: 0x0006A49F
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.CanApply(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x0600A2E2 RID: 41698 RVA: 0x0006C2B5 File Offset: 0x0006A4B5
		public override string ExplanationPart(StatRequest req)
		{
			if (this.CanApply(req))
			{
				return "StatsReport_HasHediffExplanation".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A2E3 RID: 41699 RVA: 0x002F64D0 File Offset: 0x002F46D0
		private bool CanApply(StatRequest req)
		{
			Pawn pawn;
			if (!req.HasThing || (pawn = (req.Thing as Pawn)) == null)
			{
				return false;
			}
			if (!pawn.def.race.Animal || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ExecutionCut, false))
			{
				return false;
			}
			int num = 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (typeof(Hediff_Injury).IsAssignableFrom(hediffs[i].def.hediffClass) && !hediffs[i].IsPermanent())
				{
					num++;
					if (num > 1)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04006E9E RID: 28318
		private float factor;
	}
}
