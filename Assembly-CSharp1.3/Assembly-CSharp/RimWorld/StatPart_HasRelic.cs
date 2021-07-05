using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D1 RID: 5329
	public class StatPart_HasRelic : StatPart
	{
		// Token: 0x06007F1D RID: 32541 RVA: 0x002CF776 File Offset: 0x002CD976
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val += this.offset;
			}
		}

		// Token: 0x06007F1E RID: 32542 RVA: 0x002CF79C File Offset: 0x002CD99C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReports_HasRelic".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x002CF7F4 File Offset: 0x002CD9F4
		private bool ActiveFor(Thing thing)
		{
			CompRelicContainer compRelicContainer = thing.TryGetComp<CompRelicContainer>();
			return compRelicContainer != null && compRelicContainer.ContainedThing != null;
		}

		// Token: 0x04004F70 RID: 20336
		public float offset;
	}
}
