using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D3 RID: 5331
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x06007F25 RID: 32549 RVA: 0x002CF8BC File Offset: 0x002CDABC
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06007F26 RID: 32550 RVA: 0x002CF8DC File Offset: 0x002CDADC
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F27 RID: 32551 RVA: 0x002CF924 File Offset: 0x002CDB24
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
