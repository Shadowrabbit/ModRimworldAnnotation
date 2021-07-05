using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E2 RID: 5346
	public class StatPart_Resting : StatPart
	{
		// Token: 0x06007F66 RID: 32614 RVA: 0x002D07BC File Offset: 0x002CE9BC
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

		// Token: 0x06007F67 RID: 32615 RVA: 0x002D07F0 File Offset: 0x002CE9F0
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

		// Token: 0x06007F68 RID: 32616 RVA: 0x002D0844 File Offset: 0x002CEA44
		private float RestingMultiplier(Pawn pawn)
		{
			if (pawn.InBed() || (pawn.GetPosture() != PawnPosture.Standing && !pawn.Downed) || (pawn.IsCaravanMember() && !pawn.GetCaravan().pather.MovingNow) || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x04004F93 RID: 20371
		public float factor = 1f;
	}
}
