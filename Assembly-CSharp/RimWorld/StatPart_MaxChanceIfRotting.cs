using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3E RID: 7486
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x0600A2A9 RID: 41641 RVA: 0x0006C0D9 File Offset: 0x0006A2D9
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x0600A2AA RID: 41642 RVA: 0x0006C0EB File Offset: 0x0006A2EB
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsRotting(req))
			{
				return "StatsReport_NotFresh".Translate() + ": " + 1f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A2AB RID: 41643 RVA: 0x0006C120 File Offset: 0x0006A320
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() > RotStage.Fresh;
		}
	}
}
