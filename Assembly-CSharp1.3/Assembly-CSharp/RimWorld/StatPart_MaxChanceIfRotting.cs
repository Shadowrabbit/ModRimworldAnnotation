using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D6 RID: 5334
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x06007F31 RID: 32561 RVA: 0x002CFB06 File Offset: 0x002CDD06
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x06007F32 RID: 32562 RVA: 0x002CFB18 File Offset: 0x002CDD18
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsRotting(req))
			{
				return "StatsReport_NotFresh".Translate() + ": " + 1f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F33 RID: 32563 RVA: 0x002CFB4D File Offset: 0x002CDD4D
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() > RotStage.Fresh;
		}
	}
}
