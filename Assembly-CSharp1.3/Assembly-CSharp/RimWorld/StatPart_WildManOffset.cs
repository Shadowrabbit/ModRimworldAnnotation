using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EA RID: 5354
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x06007F84 RID: 32644 RVA: 0x002D0F05 File Offset: 0x002CF105
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x06007F85 RID: 32645 RVA: 0x002D0F1B File Offset: 0x002CF11B
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsWildMan(req))
			{
				return "StatsReport_WildMan".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x06007F86 RID: 32646 RVA: 0x002D0F58 File Offset: 0x002CF158
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x04004F9A RID: 20378
		public float offset;
	}
}
