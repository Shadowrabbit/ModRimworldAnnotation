using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DD RID: 5341
	public class StatPart_PlayerFactionLeader : StatPart
	{
		// Token: 0x06007F50 RID: 32592 RVA: 0x002D00C8 File Offset: 0x002CE2C8
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.PawnIsLeader(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x06007F51 RID: 32593 RVA: 0x002D00E0 File Offset: 0x002CE2E0
		private bool PawnIsLeader(StatRequest req)
		{
			Thing thing = req.Thing;
			if (thing == null)
			{
				return false;
			}
			Faction faction = thing.Faction;
			return faction != null && faction.IsPlayer && faction.leader == thing;
		}

		// Token: 0x06007F52 RID: 32594 RVA: 0x002D0117 File Offset: 0x002CE317
		public override string ExplanationPart(StatRequest req)
		{
			if (this.PawnIsLeader(req))
			{
				return "StatsReport_LeaderOffset".Translate() + ": " + this.offset.ToStringWithSign("0.#%");
			}
			return null;
		}

		// Token: 0x04004F77 RID: 20343
		private float offset;
	}
}
