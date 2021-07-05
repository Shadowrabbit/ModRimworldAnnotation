using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B5 RID: 5045
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x06007AC6 RID: 31430 RVA: 0x002B66CC File Offset: 0x002B48CC
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06007AC7 RID: 31431 RVA: 0x002B66F0 File Offset: 0x002B48F0
		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null)
			{
				return pawn.health.summaryHealth.SummaryHealthPercent;
			}
			if (!anyThing.def.useHitPoints || !anyThing.def.healthAffectsPrice)
			{
				return 1f;
			}
			return (float)anyThing.HitPoints / (float)anyThing.MaxHitPoints;
		}
	}
}
