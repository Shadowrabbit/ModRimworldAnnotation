using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D5 RID: 5333
	public class StatPart_Malnutrition : StatPart
	{
		// Token: 0x06007F2D RID: 32557 RVA: 0x002CFA30 File Offset: 0x002CDC30
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			float num2;
			if (this.TryGetMalnutritionFactor(req, out num, out num2))
			{
				val *= num2;
			}
		}

		// Token: 0x06007F2E RID: 32558 RVA: 0x002CFA50 File Offset: 0x002CDC50
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			float f2;
			if (this.TryGetMalnutritionFactor(req, out f, out f2))
			{
				return "StatsReport_Malnutrition".Translate(f.ToStringPercent()) + ": x" + f2.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F2F RID: 32559 RVA: 0x002CFA9C File Offset: 0x002CDC9C
		private bool TryGetMalnutritionFactor(StatRequest req, out float malnutritionSeverity, out float factor)
		{
			factor = 0f;
			malnutritionSeverity = 0f;
			Pawn pawn;
			if (!req.HasThing || (pawn = (req.Thing as Pawn)) == null)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
			if (firstHediffOfDef == null)
			{
				return false;
			}
			malnutritionSeverity = firstHediffOfDef.Severity;
			factor = this.curve.Evaluate(malnutritionSeverity);
			return true;
		}

		// Token: 0x04004F71 RID: 20337
		private SimpleCurve curve;
	}
}
