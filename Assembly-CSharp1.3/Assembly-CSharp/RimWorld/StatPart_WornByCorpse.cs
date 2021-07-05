using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EB RID: 5355
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x06007F88 RID: 32648 RVA: 0x002D0F80 File Offset: 0x002CF180
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

		// Token: 0x06007F89 RID: 32649 RVA: 0x002D0FB8 File Offset: 0x002CF1B8
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

		// Token: 0x04004F9B RID: 20379
		private const float Factor = 0.1f;
	}
}
