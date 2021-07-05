using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C4 RID: 5316
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x06007EE9 RID: 32489 RVA: 0x002CEEA0 File Offset: 0x002CD0A0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.BedMultiplier(pawn);
				}
			}
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x002CEED4 File Offset: 0x002CD0D4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_InBed".Translate() + ": x" + this.BedMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x002CEF2E File Offset: 0x002CD12E
		private float BedMultiplier(Pawn pawn)
		{
			if (pawn.InBed())
			{
				return pawn.CurrentBed().GetStatValue(this.stat, true);
			}
			if (pawn.InCaravanBed())
			{
				return pawn.CurrentCaravanBed().GetStatValue(this.stat, true);
			}
			return 1f;
		}

		// Token: 0x04004F64 RID: 20324
		private StatDef stat;
	}
}
