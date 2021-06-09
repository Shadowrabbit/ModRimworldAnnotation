using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D48 RID: 7496
	public class StatPart_Resting : StatPart
	{
		// Token: 0x0600A2D9 RID: 41689 RVA: 0x002F6348 File Offset: 0x002F4548
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.RestingMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600A2DA RID: 41690 RVA: 0x002F637C File Offset: 0x002F457C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Resting".Translate() + ": x" + this.RestingMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600A2DB RID: 41691 RVA: 0x002F63D0 File Offset: 0x002F45D0
		private float RestingMultiplier(Pawn pawn)
		{
			if (pawn.InBed() || (pawn.GetPosture() != PawnPosture.Standing && !pawn.Downed) || (pawn.IsCaravanMember() && !pawn.GetCaravan().pather.MovingNow) || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x04006E9A RID: 28314
		public float factor = 1f;
	}
}
