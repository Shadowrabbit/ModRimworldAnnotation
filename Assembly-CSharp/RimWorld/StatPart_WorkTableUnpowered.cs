using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D53 RID: 7507
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x0600A30A RID: 41738 RVA: 0x0006C437 File Offset: 0x0006A637
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x0600A30B RID: 41739 RVA: 0x002F6B10 File Offset: 0x002F4D10
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				float unpoweredWorkTableWorkSpeedFactor = req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
				return "NoPower".Translate() + ": x" + unpoweredWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A30C RID: 41740 RVA: 0x002F6B74 File Offset: 0x002F4D74
		public static bool Applies(Thing th)
		{
			if (th.def.building.unpoweredWorkTableWorkSpeedFactor == 0f)
			{
				return false;
			}
			CompPowerTrader compPowerTrader = th.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && !compPowerTrader.PowerOn;
		}
	}
}
