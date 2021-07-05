using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D9 RID: 5337
	public class StatPart_NotCarefullySlaughtered : StatPart
	{
		// Token: 0x06007F3F RID: 32575 RVA: 0x002CFD0E File Offset: 0x002CDF0E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.HasWounds(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x06007F40 RID: 32576 RVA: 0x002CFD24 File Offset: 0x002CDF24
		public override string ExplanationPart(StatRequest req)
		{
			if (this.HasWounds(req))
			{
				return "StatsReport_HasHediffExplanation".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F41 RID: 32577 RVA: 0x002CFD5C File Offset: 0x002CDF5C
		private bool HasWounds(StatRequest req)
		{
			Pawn pawn;
			if (!req.HasThing || (pawn = (req.Thing as Pawn)) == null)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def != HediffDefOf.ExecutionCut && typeof(Hediff_Injury).IsAssignableFrom(hediffs[i].def.hediffClass) && !hediffs[i].IsPermanent())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004F73 RID: 20339
		private float factor;
	}
}
