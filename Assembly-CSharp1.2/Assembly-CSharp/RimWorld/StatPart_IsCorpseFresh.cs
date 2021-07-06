using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3B RID: 7483
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x0600A29D RID: 41629 RVA: 0x002F5904 File Offset: 0x002F3B04
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600A29E RID: 41630 RVA: 0x002F5924 File Offset: 0x002F3B24
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A29F RID: 41631 RVA: 0x002F596C File Offset: 0x002F3B6C
		private bool TryGetIsFreshFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Corpse corpse = req.Thing as Corpse;
			if (corpse == null)
			{
				factor = 1f;
				return false;
			}
			factor = ((corpse.GetRotStage() == RotStage.Fresh) ? 1f : 0f);
			return true;
		}
	}
}
