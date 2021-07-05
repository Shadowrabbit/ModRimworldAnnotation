using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EE RID: 5358
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x06007F95 RID: 32661 RVA: 0x002D11A2 File Offset: 0x002CF3A2
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x06007F96 RID: 32662 RVA: 0x002D11D8 File Offset: 0x002CF3D8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				float unpoweredWorkTableWorkSpeedFactor = req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
				return "NoPower".Translate() + ": x" + unpoweredWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F97 RID: 32663 RVA: 0x002D123C File Offset: 0x002CF43C
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
