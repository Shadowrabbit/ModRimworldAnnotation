using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DB RID: 5339
	public class StatPart_Pain : StatPart
	{
		// Token: 0x06007F48 RID: 32584 RVA: 0x002CFF10 File Offset: 0x002CE110
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn;
			if ((pawn = (req.Thing as Pawn)) == null)
			{
				return;
			}
			val *= this.PainFactor(pawn);
		}

		// Token: 0x06007F49 RID: 32585 RVA: 0x002CFF3A File Offset: 0x002CE13A
		public float PainFactor(Pawn pawn)
		{
			return 1f + pawn.health.hediffSet.PainTotal * this.factor;
		}

		// Token: 0x06007F4A RID: 32586 RVA: 0x002CFF5C File Offset: 0x002CE15C
		public override string ExplanationPart(StatRequest req)
		{
			Pawn pawn;
			if (req.HasThing && (pawn = (req.Thing as Pawn)) != null)
			{
				return "StatsReport_Pain".Translate() + (": " + this.PainFactor(pawn).ToStringPercent("F0"));
			}
			return null;
		}

		// Token: 0x04004F76 RID: 20342
		private float factor = 1f;
	}
}
