using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D50 RID: 7504
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x0600A2FD RID: 41725 RVA: 0x002F6970 File Offset: 0x002F4B70
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					val *= 0.1f;
				}
			}
		}

		// Token: 0x0600A2FE RID: 41726 RVA: 0x002F69A8 File Offset: 0x002F4BA8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					return "StatsReport_WornByCorpse".Translate() + ": x" + 0.1f.ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x04006EA7 RID: 28327
		private const float Factor = 0.1f;
	}
}
