using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C9 RID: 5321
	public class StatPart_CorpseCasket : StatPart
	{
		// Token: 0x06007EFA RID: 32506 RVA: 0x002CF26A File Offset: 0x002CD46A
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ApplyTo(req))
			{
				val += (float)this.offsetOccupied;
			}
		}

		// Token: 0x06007EFB RID: 32507 RVA: 0x002CF281 File Offset: 0x002CD481
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ApplyTo(req))
			{
				return null;
			}
			return "StatsReport_OccupiedCorpseCasket".Translate() + ": " + this.offsetOccupied;
		}

		// Token: 0x06007EFC RID: 32508 RVA: 0x002CF2B8 File Offset: 0x002CD4B8
		private bool ApplyTo(StatRequest req)
		{
			Building_CorpseCasket building_CorpseCasket;
			return (building_CorpseCasket = (req.Thing as Building_CorpseCasket)) != null && building_CorpseCasket.HasCorpse && this.offsetOccupied != 0 && (this.thingDefs == null || this.thingDefs.Contains(req.Thing.def));
		}

		// Token: 0x04004F65 RID: 20325
		public int offsetOccupied;

		// Token: 0x04004F66 RID: 20326
		public List<ThingDef> thingDefs;
	}
}
