using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2C RID: 7468
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x0600A25E RID: 41566 RVA: 0x002F5334 File Offset: 0x002F3534
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

		// Token: 0x0600A25F RID: 41567 RVA: 0x002F5368 File Offset: 0x002F3568
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

		// Token: 0x0600A260 RID: 41568 RVA: 0x0006BDE3 File Offset: 0x00069FE3
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

		// Token: 0x04006E67 RID: 28263
		private StatDef stat;
	}
}
