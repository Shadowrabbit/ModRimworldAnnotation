using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E5 RID: 5349
	public class StatPart_Slave : StatPart
	{
		// Token: 0x06007F71 RID: 32625 RVA: 0x002D0A4D File Offset: 0x002CEC4D
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.factor;
			}
		}

		// Token: 0x06007F72 RID: 32626 RVA: 0x002D0A6C File Offset: 0x002CEC6C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_Slave".Translate() + (": x" + this.factor.ToStringPercent());
			}
			return null;
		}

		// Token: 0x06007F73 RID: 32627 RVA: 0x002D0ABC File Offset: 0x002CECBC
		private bool ActiveFor(Thing t)
		{
			Pawn pawn;
			return (pawn = (t as Pawn)) != null && pawn.IsSlave;
		}

		// Token: 0x04004F97 RID: 20375
		private float factor = 0.75f;
	}
}
